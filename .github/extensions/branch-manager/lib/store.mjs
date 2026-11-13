import { mkdir, open, readFile, rename, stat, unlink } from "node:fs/promises";
import { watch } from "node:fs";
import { randomBytes } from "node:crypto";
import { homedir, hostname } from "node:os";
import path from "node:path";
import { setTimeout as sleep } from "node:timers/promises";

const STATE_VERSION = 1;

const LOCK_TIMEOUT_MS = 5_000;
// A lock is only considered abandoned this long after it was taken, even if its owner looks dead
// or its owner is unknown, so a holder that is merely slow is never overridden early.
const LOCK_STALE_MS = 30_000;
// A lock file that exists but has no readable owner is normally one that is still being written.
const LOCK_UNREADABLE_STALE_MS = 2_000;
const RECLAIM_STALE_MS = 10_000;
// Windows reports these while another process briefly holds or is deleting a file.
const TRANSIENT_CODES = new Set(["EPERM", "EBUSY", "EACCES"]);

async function retryTransient(operation, attempts = 8) {
    for (let attempt = 1; ; attempt++) {
        try {
            return await operation();
        } catch (error) {
            if (!TRANSIENT_CODES.has(error.code) || attempt >= attempts) {
                throw error;
            }
            await sleep(15 * attempt);
        }
    }
}

function processIsAlive(pid) {
    if (!Number.isInteger(pid) || pid <= 0) {
        return false;
    }
    try {
        process.kill(pid, 0);
        return true;
    } catch (error) {
        return error.code === "EPERM";
    }
}

function parseOwner(raw) {
    try {
        const owner = JSON.parse(raw);
        return owner && typeof owner === "object" ? owner : null;
    } catch {
        return null;
    }
}

function lockIsStale(raw, ageMs) {
    const owner = parseOwner(raw);
    if (!owner) {
        return ageMs > LOCK_UNREADABLE_STALE_MS;
    }
    if (ageMs > LOCK_STALE_MS) {
        return true;
    }
    return owner.host === hostname() && !processIsAlive(owner.pid);
}

function emptyState() {
    return {
        version: STATE_VERSION,
        // null means "use the defaults derived from the repo's branches".
        branches: null,
        sortBranch: null,
        // Manual associations: each group is a set of commit SHAs that are the same change.
        links: [],
        // Commits the user explicitly separated; excluded from automatic matching.
        detached: [],
    };
}

/**
 * Link state is shared by every checkout and worktree of a repo, so it lives under the
 * user's Copilot home rather than in the repo or the session workspace.
 */
export function stateDirectory() {
    const home = process.env.COPILOT_HOME || path.join(homedir(), ".copilot");
    return path.join(home, "extensions", "branch-manager", "artifacts");
}

export function createStore(repoKey) {
    const directory = stateDirectory();
    const file = path.join(directory, `${repoKey}.json`);
    const lockFile = `${file}.lock`;
    const reclaimFile = `${lockFile}.reclaim`;
    const listeners = new Set();
    let queue = Promise.resolve();
    let watcher = null;
    let debounce = null;

    async function read() {
        try {
            const parsed = JSON.parse(await retryTransient(() => readFile(file, "utf8")));
            return { ...emptyState(), ...parsed };
        } catch (error) {
            if (error.code === "ENOENT") {
                return emptyState();
            }
            throw new Error(`Could not read ${file}: ${error.message}`);
        }
    }

    async function write(state) {
        await mkdir(directory, { recursive: true });
        const temp = `${file}.${process.pid}.${randomBytes(6).toString("hex")}.tmp`;
        try {
            const handle = await open(temp, "wx");
            try {
                await handle.writeFile(JSON.stringify(state, null, 2) + "\n", "utf8");
                await handle.sync();
            } finally {
                await handle.close();
            }
            await retryTransient(() => rename(temp, file));
        } catch (error) {
            await unlink(temp).catch(() => {});
            throw error;
        }
    }

    async function readLock() {
        try {
            const handle = await open(lockFile, "r");
            try {
                const [stats, raw] = await Promise.all([handle.stat(), handle.readFile("utf8")]);
                return { raw, ageMs: Date.now() - stats.mtimeMs };
            } finally {
                await handle.close();
            }
        } catch {
            return null;
        }
    }

    /** Removes an abandoned lock; only one process at a time may do this, and it re-checks under that guard. */
    async function reclaimStaleLock(staleRaw) {
        let guard;
        try {
            guard = await open(reclaimFile, "wx");
        } catch (error) {
            if (error.code === "EEXIST") {
                const stats = await stat(reclaimFile).catch(() => null);
                if (stats && Date.now() - stats.mtimeMs > RECLAIM_STALE_MS) {
                    await unlink(reclaimFile).catch(() => {});
                }
            }
            return;
        }
        try {
            await guard.close();
            const current = await readLock();
            if (current && current.raw === staleRaw && lockIsStale(current.raw, current.ageMs)) {
                await unlink(lockFile).catch(() => {});
            }
        } finally {
            await unlink(reclaimFile).catch(() => {});
        }
    }

    async function acquireLock() {
        await mkdir(directory, { recursive: true });
        const token = randomBytes(8).toString("hex");
        const owner = JSON.stringify({
            pid: process.pid,
            host: hostname(),
            token,
            createdAt: new Date().toISOString(),
        });
        const deadline = Date.now() + LOCK_TIMEOUT_MS;
        let lastError = null;

        for (;;) {
            let handle = null;
            try {
                handle = await open(lockFile, "wx");
            } catch (error) {
                if (error.code !== "EEXIST" && !TRANSIENT_CODES.has(error.code)) {
                    throw error;
                }
                lastError = error;
            }

            if (handle) {
                try {
                    await handle.writeFile(owner, "utf8");
                } catch (error) {
                    await handle.close().catch(() => {});
                    await unlink(lockFile).catch(() => {});
                    throw error;
                }
                await handle.close();
                return token;
            }

            const existing = await readLock();
            if (existing && lockIsStale(existing.raw, existing.ageMs)) {
                await reclaimStaleLock(existing.raw);
            }
            if (Date.now() >= deadline) {
                const detail = lastError && lastError.code !== "EEXIST" ? ` (${lastError.message})` : "";
                throw new Error(
                    `Could not update saved links: another Copilot session is holding the lock on ${file}${detail}. Try again in a moment.`,
                );
            }
            await sleep(10 + Math.random() * 40);
        }
    }

    async function releaseLock(token) {
        try {
            await retryTransient(async () => {
                const current = await readFile(lockFile, "utf8");
                if (parseOwner(current)?.token === token) {
                    await unlink(lockFile);
                }
            });
        } catch {}
    }

    /** Read-modify-write under a lock that is shared with other processes, so concurrent sessions can't lose each other's changes. */
    function update(mutator) {
        const run = queue.then(async () => {
            const token = await acquireLock();
            try {
                const state = await read();
                const result = (await mutator(state)) ?? state;
                await write(state);
                return result;
            } finally {
                await releaseLock(token);
            }
        });
        queue = run.catch(() => {});
        return run;
    }

    function startWatching() {
        if (watcher) {
            return;
        }
        mkdir(directory, { recursive: true })
            .then(() => {
                const name = path.basename(file);
                watcher = watch(directory, { persistent: false }, (_event, changed) => {
                    if (changed !== name) {
                        return;
                    }
                    clearTimeout(debounce);
                    debounce = setTimeout(() => listeners.forEach((fn) => fn()), 150);
                });
                watcher.on("error", () => {});
            })
            .catch(() => {});
    }

    return {
        file,
        read,
        update,
        onChange(fn) {
            listeners.add(fn);
            startWatching();
            return () => listeners.delete(fn);
        },
        close() {
            clearTimeout(debounce);
            watcher?.close();
            watcher = null;
            listeners.clear();
        },
    };
}
