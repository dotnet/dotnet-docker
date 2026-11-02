import { mkdir, readFile, rename, writeFile } from "node:fs/promises";
import { watch } from "node:fs";
import { homedir } from "node:os";
import path from "node:path";

const STATE_VERSION = 1;

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
    return path.join(home, "extensions", "porting-board", "artifacts");
}

export function createStore(repoKey) {
    const directory = stateDirectory();
    const file = path.join(directory, `${repoKey}.json`);
    const listeners = new Set();
    let queue = Promise.resolve();
    let watcher = null;
    let debounce = null;

    async function read() {
        try {
            const parsed = JSON.parse(await readFile(file, "utf8"));
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
        const temp = `${file}.${process.pid}.tmp`;
        await writeFile(temp, JSON.stringify(state, null, 2) + "\n", "utf8");
        await rename(temp, file);
    }

    /** Serialized read-modify-write so concurrent actions in this process can't clobber each other. */
    function update(mutator) {
        const run = queue.then(async () => {
            const state = await read();
            const result = (await mutator(state)) ?? state;
            await write(state);
            return result;
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
