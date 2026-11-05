import { execFile } from "node:child_process";
import { selectChanges } from "./history.mjs";

const FIELD = "\x1f";
const RECORD = "\x1e";

export function runCommand(file, cwd, args, { timeout = 120_000 } = {}) {
    return new Promise((resolve, reject) => {
        execFile(
            file,
            args,
            {
                cwd,
                timeout,
                windowsHide: true,
                maxBuffer: 256 * 1024 * 1024,
                env: { ...process.env, GIT_TERMINAL_PROMPT: "0", GH_PROMPT_DISABLED: "1" },
            },
            (err, stdout, stderr) => {
                if (err) {
                    reject(new Error((stderr || err.message).trim()));
                } else {
                    resolve(stdout);
                }
            },
        );
    });
}

export const runGit = (cwd, args, options) => runCommand("git", cwd, args, options);

export async function describeRepo(cwd, remote = "origin") {
    const root = (await runGit(cwd, ["rev-parse", "--show-toplevel"])).trim();

    let url = "";
    try {
        url = (await runGit(root, ["remote", "get-url", remote])).trim();
    } catch {
        // No such remote; fall back to the checkout path for identity.
    }

    const match = /github\.com[:/]([^/\s]+)\/([^/\s]+?)(?:\.git)?\/?$/i.exec(url);
    const slug = match ? `${match[1]}/${match[2]}` : null;
    const key = (slug ?? (url || root)).toLowerCase().replace(/[^a-z0-9._-]+/g, "_");
    return { root, remote, url, slug, key };
}

/** Known branches: remote-tracking refs for `remote` and local heads. */
export async function listRefs(root, remote) {
    const out = await runGit(root, [
        "for-each-ref",
        "--format=%(refname)",
        `refs/remotes/${remote}`,
        "refs/heads",
    ]);

    const remoteBranches = new Set();
    const localBranches = new Set();
    const remotePrefix = `refs/remotes/${remote}/`;
    for (const line of out.split("\n")) {
        const ref = line.trim();
        if (ref.startsWith(remotePrefix)) {
            const name = ref.slice(remotePrefix.length);
            if (name !== "HEAD") {
                remoteBranches.add(name);
            }
        } else if (ref.startsWith("refs/heads/")) {
            localBranches.add(ref.slice("refs/heads/".length));
        }
    }

    let defaultBranch = null;
    try {
        const head = (await runGit(root, ["symbolic-ref", `refs/remotes/${remote}/HEAD`])).trim();
        defaultBranch = head.slice(remotePrefix.length);
    } catch {
        // origin/HEAD is not always set.
    }

    return { remoteBranches, localBranches, defaultBranch };
}

/** Branches that exist on the remote itself, including ones never fetched locally. */
export async function listRemoteBranches(root, remote) {
    const out = await runGit(root, ["ls-remote", "--heads", remote], { timeout: 60_000 });
    return out
        .split("\n")
        .map((line) => line.split("\t")[1]?.trim())
        .filter((ref) => ref?.startsWith("refs/heads/"))
        .map((ref) => ref.slice("refs/heads/".length));
}

/** Prefer the remote-tracking ref; fall back to a local branch of the same name. */
export function resolveRef(refs, remote, branch) {
    if (refs.remoteBranches.has(branch)) {
        return `refs/remotes/${remote}/${branch}`;
    }
    if (refs.localBranches.has(branch)) {
        return `refs/heads/${branch}`;
    }
    return null;
}

export async function readCommits(root, ref, sinceIso) {
    const format = ["%H", "%P", "%an", "%cI", "%s", "%b"].join(FIELD) + RECORD;
    const out = await runGit(root, [
        "log",
        ref,
        `--since=${sinceIso}`,
        "--max-count=5000",
        `--format=${format}`,
        "--",
    ]);

    const records = [];
    for (const record of out.split(RECORD)) {
        const text = record.replace(/^\s+/, "");
        if (!text) {
            continue;
        }
        const [sha, parents, author, date, subject, body = ""] = text.split(FIELD);
        records.push({ sha, parents: parents ? parents.split(" ") : [], author, date, subject, body });
    }
    return selectChanges(records);
}

export async function hasOlderCommits(root, ref, sinceIso) {
    const out = await runGit(root, [
        "log",
        ref,
        "--no-merges",
        `--before=${sinceIso}`,
        "-1",
        "--format=%H",
        "--",
    ]);
    return out.trim().length > 0;
}

/**
 * Expands abbreviated SHAs to full commit SHAs; throws if any can't be resolved.
 * Open pull requests are identified as `pr:<number>` and pass through unchanged.
 */
export async function expandShas(root, shas) {
    return Promise.all(
        shas.map(async (sha) => {
            if (/^pr:\d+$/i.test(sha)) {
                return sha.toLowerCase();
            }
            if (!/^[0-9a-f]{7,40}$/i.test(sha)) {
                throw new Error(`Not a commit SHA or pull request: ${sha}`);
            }
            return (await runGit(root, ["rev-parse", "--verify", "--quiet", `${sha}^{commit}`])).trim();
        }),
    );
}

export async function fetchBranches(root, remote, branches) {
    const refspec = (b) => `+refs/heads/${b}:refs/remotes/${remote}/${b}`;
    const baseArgs = ["fetch", "--no-tags", "--no-write-fetch-head", remote];

    try {
        await runGit(root, [...baseArgs, ...branches.map(refspec)], { timeout: 180_000 });
        return { failed: [] };
    } catch {
        // One missing branch fails the whole fetch; retry individually.
    }

    const failed = [];
    for (const branch of branches) {
        try {
            await runGit(root, [...baseArgs, refspec(branch)], { timeout: 180_000 });
        } catch (error) {
            failed.push({ branch, message: error.message });
        }
    }
    return { failed };
}
