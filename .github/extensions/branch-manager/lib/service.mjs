import { randomUUID } from "node:crypto";
import { buildBoard, defaultBranches } from "./board.mjs";
import {
    describeRepo,
    expandShas,
    fetchBranches,
    hasOlderCommits,
    listRefs,
    listRemoteBranches,
    readCommits,
    resolveRef,
} from "./git.mjs";
import { listOpenPullRequests } from "./github.mjs";
import { buildPortPrompt } from "./prompt.mjs";
import { openUrl as openInBrowser } from "./open.mjs";
import { createStore } from "./store.mjs";

const MAX_MONTHS = 60;
const PULL_REQUEST_TTL_MS = 5 * 60_000;
const PULL_REQUEST_ERROR_TTL_MS = 30_000;
const REMOTE_BRANCH_TTL_MS = 5 * 60_000;

/**
 * Board operations shared by the HTTP API (the UI) and the canvas actions (the agent).
 */
export function createService({ getWorkingDirectories, sendPrompt, openUrl = openInBrowser }) {
    const listeners = new Set();
    let context = null;
    let lastBoard = null;
    let lastFetchedAt = null;
    let refreshing = null;
    let remoteBranches = null;
    const knownChanges = new Map();
    const pullRequestCache = new Map();
    const pullRequestRequests = new Map();

    const emit = () => listeners.forEach((fn) => fn());

    /** Branch names on the remote, cached because it's a network round trip. Failures yield none. */
    async function loadRemoteBranches(repo) {
        const ttl = remoteBranches?.failed ? PULL_REQUEST_ERROR_TTL_MS : REMOTE_BRANCH_TTL_MS;
        if (!remoteBranches || Date.now() - remoteBranches.at >= ttl) {
            remoteBranches = await listRemoteBranches(repo.root, repo.remote).then(
                (names) => ({ at: Date.now(), names }),
                () => ({ at: Date.now(), names: [], failed: true }),
            );
        }
        return remoteBranches.names;
    }

    /** Open PRs for a branch, cached briefly because `gh` is a network round trip. */
    function loadPullRequests(repo, branch) {
        const cached = pullRequestCache.get(branch);
        const ttl = cached?.error ? PULL_REQUEST_ERROR_TTL_MS : PULL_REQUEST_TTL_MS;
        if (cached && Date.now() - cached.at < ttl) {
            return Promise.resolve(cached);
        }
        if (!pullRequestRequests.has(branch)) {
            const request = repo.slug
                ? listOpenPullRequests(repo.root, repo.slug, branch)
                : Promise.reject(new Error("The remote isn't a GitHub repository."));
            pullRequestRequests.set(
                branch,
                request
                    .then(
                        (prs) => ({ at: Date.now(), prs }),
                        (error) => ({
                            at: Date.now(),
                            prs: [],
                            error: /ENOENT/.test(error.message) ? "The GitHub CLI (gh) isn't installed." : error.message,
                        }),
                    )
                    .then((entry) => {
                        pullRequestCache.set(branch, entry);
                        pullRequestRequests.delete(branch);
                        return entry;
                    }),
            );
        }
        return pullRequestRequests.get(branch);
    }

    async function getContext() {
        if (!context) {
            let repo = null;
            let failure = null;
            for (const directory of getWorkingDirectories()) {
                try {
                    repo = await describeRepo(directory);
                    break;
                } catch (error) {
                    failure ??= error;
                }
            }
            if (!repo) {
                throw new Error(`Not inside a git repository. ${failure?.message ?? ""}`.trim());
            }
            const store = createStore(repo.key);
            store.onChange(emit);
            context = { repo, store };
        }
        return context;
    }

    async function loadSettings() {
        const { repo, store } = await getContext();
        const [state, refs, remoteNames] = await Promise.all([
            store.read(),
            listRefs(repo.root, repo.remote),
            loadRemoteBranches(repo),
        ]);
        // The remote is authoritative when reachable: local refs include stale tracking refs and unrelated local branches.
        refs.knownBranches = remoteNames.length
            ? remoteNames
            : [...new Set([...refs.remoteBranches, ...refs.localBranches])];
        const branches = state.branches?.length ? state.branches : defaultBranches(refs);
        const sortBranch = branches.includes(state.sortBranch) ? state.sortBranch : (branches[0] ?? null);
        return { repo, store, state, refs, branches, sortBranch };
    }

    async function getBoard({ months = 1 } = {}) {
        const { repo, state, refs, branches, sortBranch } = await loadSettings();
        const span = Math.min(Math.max(1, Math.floor(months) || 1), MAX_MONTHS);
        const since = new Date();
        since.setMonth(since.getMonth() - span);
        const sinceIso = since.toISOString();

        const [loaded, pullRequestResults] = await Promise.all([
            Promise.all(
                branches.map(async (name) => {
                    const ref = resolveRef(refs, repo.remote, name);
                    if (!ref) {
                        return { name, commits: [], hasOlder: false, error: "Not fetched yet. Try Refresh." };
                    }
                    try {
                        const [commits, hasOlder] = await Promise.all([
                            readCommits(repo.root, ref, sinceIso),
                            hasOlderCommits(repo.root, ref, sinceIso),
                        ]);
                        return { name, commits, hasOlder };
                    } catch (error) {
                        return { name, commits: [], hasOlder: false, error: error.message };
                    }
                }),
            ),
            Promise.all(branches.map((name) => loadPullRequests(repo, name))),
        ]);

        const pullRequests = pullRequestResults.flatMap((result) => result.prs);
        const { rows, commits } = buildBoard({
            commitsByBranch: Object.fromEntries(loaded.map((b) => [b.name, b.commits])),
            pullRequests,
            branches,
            sortBranch,
            manualGroups: state.links,
            detached: state.detached,
        });

        for (const change of Object.values(commits)) {
            if (change.kind === "pr") {
                change.short = `#${change.pr}`;
                change.prUrl = change.url;
            } else {
                change.short = change.sha.slice(0, 7);
                if (repo.slug) {
                    change.url = `https://github.com/${repo.slug}/commit/${change.sha}`;
                    if (change.pr !== null) {
                        change.prUrl = `https://github.com/${repo.slug}/pull/${change.pr}`;
                    }
                }
            }
            // Kept across reloads so a dialog opened on an older board can still be composed.
            knownChanges.set(change.sha, change);
        }

        lastBoard = {
            repo: { slug: repo.slug, remote: repo.remote },
            branches: loaded.map(({ name, commits: list, hasOlder, error }) => ({
                name,
                count: list.length,
                hasOlder,
                error,
            })),
            availableBranches: refs.knownBranches,
            sortBranch,
            months: span,
            since: sinceIso,
            hasMore: loaded.some((b) => b.hasOlder),
            fetchedAt: lastFetchedAt,
            openPullRequests: {
                count: pullRequests.length,
                error: pullRequestResults.find((result) => result.error)?.error,
            },
            rows,
            commits,
        };
        return lastBoard;
    }

    async function refresh() {
        refreshing ??= (async () => {
            pullRequestCache.clear();
            remoteBranches = null;
            const { repo, refs, branches } = await loadSettings();
            const { failed } = await fetchBranches(repo.root, repo.remote, branches);
            lastFetchedAt = new Date().toISOString();
            emit();
            return { fetchedAt: lastFetchedAt, failed: failed.filter((f) => !refs.localBranches.has(f.branch)) };
        })().finally(() => {
            refreshing = null;
        });
        return refreshing;
    }

    async function link(shas, { by = "user" } = {}) {
        const { repo, store } = await getContext();
        const full = [...new Set(await expandShas(repo.root, shas))];
        if (full.length < 2) {
            throw new Error("Select at least two different changes to link.");
        }
        const selected = new Set(full);

        await store.update((state) => {
            state.detached = state.detached.filter((sha) => !selected.has(sha));
            const overlapping = state.links.filter((group) => group.shas.some((sha) => selected.has(sha)));
            const untouched = state.links.filter((group) => !overlapping.includes(group));
            const merged = new Set([...selected, ...overlapping.flatMap((group) => group.shas)]);
            untouched.push({ id: randomUUID(), shas: [...merged], createdAt: new Date().toISOString(), createdBy: by });
            state.links = untouched;
        });
        emit();
        return { linked: full };
    }

    async function unlink(shas) {
        const { repo, store } = await getContext();
        const full = [...new Set(await expandShas(repo.root, shas))];
        if (!full.length) {
            throw new Error("Select at least one commit to unlink.");
        }
        const removed = new Set(full);

        await store.update((state) => {
            state.links = state.links
                .map((group) => ({ ...group, shas: group.shas.filter((sha) => !removed.has(sha)) }))
                .filter((group) => group.shas.length > 1);
            for (const sha of full) {
                if (!state.detached.includes(sha)) {
                    state.detached.push(sha);
                }
            }
        });
        emit();
        return { unlinked: full };
    }

    async function setConfig({ branches, sortBranch }) {
        const { store } = await getContext();

        await store.update((state) => {
            if (branches !== undefined) {
                const cleaned = [...new Set(branches.map((b) => String(b).trim()).filter(Boolean))];
                if (!cleaned.length) {
                    throw new Error("Track at least one branch.");
                }
                state.branches = cleaned;
                if (!cleaned.includes(state.sortBranch)) {
                    state.sortBranch = null;
                }
            }
            if (sortBranch !== undefined) {
                state.sortBranch = sortBranch;
            }
        });
        emit();
        return { ok: true };
    }

    const MAX_PROMPT_LENGTH = 100_000;

    /** Builds the prompt for porting a batch of changes to `target`, for the user to review. */
    async function composePortPrompt({ target, items }) {
        const board = lastBoard ?? (await getBoard());
        if (!board.branches.some((b) => b.name === target)) {
            throw new Error(`"${target}" is not a tracked branch.`);
        }
        if (!Array.isArray(items) || !items.length || items.length > 200) {
            throw new Error("Choose at least one change to port.");
        }

        const time = (change) => Date.parse(change.date);
        const resolved = items
            .map((item) => {
                const shas = Array.isArray(item?.shas) ? item.shas : [];
                const changes = shas.map((sha) => knownChanges.get(sha));
                if (!changes.length || changes.some((change) => !change)) {
                    throw new Error("A selected change is no longer on the board. Refresh and try again.");
                }
                const branches = Array.isArray(item.branches) ? item.branches.filter((b) => typeof b === "string") : [];
                return { branches, changes: changes.sort((a, b) => time(a) - time(b)) };
            })
            .sort((a, b) => time(a.changes[0]) - time(b.changes[0]));

        return { prompt: buildPortPrompt({ repoSlug: board.repo.slug, target, items: resolved }) };
    }

    /** Sends the (possibly user-edited) prompt to Copilot. */
    async function sendToCopilot(prompt) {
        if (typeof prompt !== "string" || !prompt.trim()) {
            throw new Error("The prompt is empty.");
        }
        if (prompt.length > MAX_PROMPT_LENGTH) {
            throw new Error("The prompt is too long.");
        }
        await sendPrompt(prompt);
        return { sent: true };
    }

    /** Opens a commit or pull request of this repo on GitHub; anything else is refused. */
    async function openOnGitHub(url) {
        const { repo } = await getContext();
        const prefix = repo.slug ? `https://github.com/${repo.slug}/` : null;
        if (!prefix || typeof url !== "string" || !url.startsWith(prefix) || !/^(commit|pull)\/[0-9a-f]+$/i.test(url.slice(prefix.length))) {
            throw new Error("Only commits and pull requests of this repository can be opened.");
        }
        await openUrl(url);
        return { opened: true };
    }

    /** Compact view for the agent: what changed, where it landed, and where it's missing. */
    async function describeBoard({ months = 1, onlyMissing = false, limit = 100 } = {}) {
        const board = await getBoard({ months });
        const names = board.branches.map((b) => b.name);
        const rows = board.rows
            .map((row) => ({
                linkedBy: row.linked,
                // Nothing has merged in a pending row, so nothing is missing from other branches yet.
                missingFrom: row.pending ? [] : names.filter((name) => row.cells[name].length === 0),
                changes: row.shas.map((sha) => {
                    const c = board.commits[sha];
                    return {
                        sha: c.sha,
                        title: c.subject,
                        pr: c.pr,
                        openPullRequest: c.kind === "pr",
                        author: c.author,
                        date: c.date,
                        branches: c.branches,
                    };
                }),
            }))
            .filter((row) => !onlyMissing || row.missingFrom.length > 0);

        return {
            repo: board.repo.slug,
            branches: names,
            since: board.since,
            totalRows: rows.length,
            rows: rows.slice(0, limit),
        };
    }

    return {
        getBoard,
        refresh,
        link,
        unlink,
        setConfig,
        composePortPrompt,
        sendToCopilot,
        openOnGitHub,
        describeBoard,
        onChange(fn) {
            listeners.add(fn);
            return () => listeners.delete(fn);
        },
        close() {
            context?.store.close();
            listeners.clear();
        },
    };
}
