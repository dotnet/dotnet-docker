const PR_SUFFIX = /\(#(\d+)\)\s*$/;
const CHERRY_PICK = /\(cherry picked from commit ([0-9a-f]{7,40})\)/gi;
const TITLE_MATCH_WINDOW_MS = 45 * 24 * 60 * 60 * 1000;
const MIN_TITLE_LENGTH = 8;

// Strongest evidence first; used to pick the single label shown on a card.
export const LINK_REASONS = ["manual", "cherry-pick", "title"];

export function extractPr(subject) {
    const match = PR_SUFFIX.exec(subject);
    return match ? Number(match[1]) : null;
}

/** Drops `[branch]` tags and the `(#123)` suffix so ports of the same PR compare equal. */
export function normalizeTitle(subject) {
    return subject
        .trim()
        .replace(PR_SUFFIX, "")
        .trim()
        .replace(/^(?:\[[^\]]+\]\s*)+/, "")
        .trim()
        .toLowerCase()
        .replace(/\s+/g, " ");
}

const isPullRequest = (node) => node.kind === "pr";
const disjoint = (a, b) => !a.some((x) => b.includes(x));

class Components {
    constructor(nodes) {
        this.parent = new Map();
        this.branches = new Map();
        this.reasons = new Map();
        for (const node of nodes) {
            this.parent.set(node.sha, node.sha);
            this.branches.set(node.sha, new Set(node.branches));
            this.reasons.set(node.sha, new Set());
        }
    }

    find(id) {
        let node = id;
        while (this.parent.get(node) !== node) {
            this.parent.set(node, this.parent.get(this.parent.get(node)));
            node = this.parent.get(node);
        }
        return node;
    }

    /** With `strict`, refuses to merge components that already have a change on the same branch. */
    union(a, b, reason, { strict = false } = {}) {
        const rootA = this.find(a);
        const rootB = this.find(b);
        if (rootA === rootB) {
            this.reasons.get(rootA).add(reason);
            return true;
        }

        const branchesA = this.branches.get(rootA);
        const branchesB = this.branches.get(rootB);
        if (strict && [...branchesA].some((branch) => branchesB.has(branch))) {
            return false;
        }

        this.parent.set(rootB, rootA);
        branchesB.forEach((branch) => branchesA.add(branch));
        this.branches.delete(rootB);
        const reasons = this.reasons.get(rootA);
        this.reasons.get(rootB).forEach((r) => reasons.add(r));
        this.reasons.delete(rootB);
        reasons.add(reason);
        return true;
    }
}

function findBySha(commits, sha) {
    if (commits.has(sha)) {
        return commits.get(sha);
    }
    if (sha.length < 7) {
        return undefined;
    }
    for (const [full, commit] of commits) {
        if (!isPullRequest(commit) && full.startsWith(sha)) {
            return commit;
        }
    }
    return undefined;
}

/**
 * Groups changes across branches into rows. A change is a commit or an open pull request.
 *
 * A commit reachable from several tracked branches (e.g. after a merge) is one commit.
 * Different changes are grouped by manual links first, then by automatic evidence
 * (`git cherry-pick -x` trailers, matching titles).
 *
 * Open pull requests are identified as `pr:<number>`. Rows that contain one are listed
 * first, because they are work in flight.
 */
export function buildBoard({
    commitsByBranch,
    pullRequests = [],
    branches,
    sortBranch,
    manualGroups = [],
    detached = [],
}) {
    const nodes = new Map();
    for (const branch of branches) {
        for (const raw of commitsByBranch[branch] ?? []) {
            let commit = nodes.get(raw.sha);
            if (!commit) {
                commit = { ...raw, pr: extractPr(raw.subject), branches: [], time: Date.parse(raw.date) };
                nodes.set(raw.sha, commit);
            }
            if (!commit.branches.includes(branch)) {
                commit.branches.push(branch);
            }
        }
    }
    for (const pr of pullRequests) {
        if (!branches.includes(pr.base)) {
            continue;
        }
        const id = `pr:${pr.number}`;
        nodes.set(id, {
            sha: id,
            kind: "pr",
            pr: pr.number,
            subject: pr.title,
            body: "",
            author: pr.author,
            date: pr.createdAt,
            time: Date.parse(pr.createdAt),
            branches: [pr.base],
            url: pr.url,
            isDraft: pr.isDraft,
        });
    }

    const byPrNumber = new Map();
    for (const node of nodes.values()) {
        if (node.pr !== null) {
            byPrNumber.set(node.pr, [...(byPrNumber.get(node.pr) ?? []), node]);
        }
    }

    // A `pr:N` token keeps meaning after the PR merges: it then refers to the merged commit.
    const resolve = (token) => {
        if (nodes.has(token)) {
            return [token];
        }
        const match = /^pr:(\d+)$/.exec(token);
        return match ? (byPrNumber.get(Number(match[1])) ?? []).map((node) => node.sha) : [];
    };

    const components = new Components(nodes.values());
    const detachedSet = new Set(detached);
    const isDetached = (node) => detachedSet.has(node.sha) || (node.pr !== null && detachedSet.has(`pr:${node.pr}`));
    const linkable = [...nodes.values()].filter((node) => !isDetached(node));

    for (const group of manualGroups) {
        const present = [...new Set(group.shas.flatMap(resolve))];
        for (let i = 1; i < present.length; i++) {
            components.union(present[0], present[i], "manual");
        }
    }

    for (const commit of linkable) {
        for (const match of commit.body.matchAll(CHERRY_PICK)) {
            const source = findBySha(nodes, match[1]);
            if (source && !isDetached(source)) {
                components.union(commit.sha, source.sha, "cherry-pick");
            }
        }
    }

    const byTitle = new Map();
    for (const node of linkable) {
        const key = normalizeTitle(node.subject);
        if (key.length >= MIN_TITLE_LENGTH) {
            byTitle.set(key, [...(byTitle.get(key) ?? []), node]);
        }
    }
    const titlePairs = [];
    for (const group of byTitle.values()) {
        for (let i = 0; i < group.length; i++) {
            for (let j = i + 1; j < group.length; j++) {
                const distance = Math.abs(group[i].time - group[j].time);
                if (distance <= TITLE_MATCH_WINDOW_MS && disjoint(group[i].branches, group[j].branches)) {
                    titlePairs.push({ a: group[i].sha, b: group[j].sha, distance });
                }
            }
        }
    }
    // Closest-in-time first so recurring bot titles pair up one-to-one per branch.
    titlePairs.sort((x, y) => x.distance - y.distance);
    for (const pair of titlePairs) {
        components.union(pair.a, pair.b, "title", { strict: true });
    }

    const grouped = new Map();
    for (const node of nodes.values()) {
        const root = components.find(node.sha);
        grouped.set(root, [...(grouped.get(root) ?? []), node]);
    }

    const rows = [];
    for (const [root, members] of grouped) {
        members.sort((x, y) => y.time - x.time || x.sha.localeCompare(y.sha));
        const open = members.some(isPullRequest);
        const merged = members.filter((m) => !isPullRequest(m));
        const onSortBranch = merged.filter((c) => c.branches.includes(sortBranch));
        const basis = open ? members.filter(isPullRequest) : onSortBranch.length ? onSortBranch : merged;
        rows.push({
            id: members.map((c) => c.sha).sort()[0],
            open,
            // Nothing merged yet, so there is nothing to port.
            pending: merged.length === 0,
            sortTime: Math.max(...basis.map((c) => c.time)),
            linked: members.length > 1 ? LINK_REASONS.filter((r) => components.reasons.get(root).has(r)) : [],
            cells: Object.fromEntries(
                branches.map((branch) => [branch, members.filter((c) => c.branches.includes(branch)).map((c) => c.sha)]),
            ),
            shas: members.map((c) => c.sha),
        });
    }
    rows.sort((x, y) => Number(y.open) - Number(x.open) || y.sortTime - x.sortTime || x.id.localeCompare(y.id));

    const publicCommits = {};
    for (const node of nodes.values()) {
        const { body, time, parents, ...rest } = node;
        publicCommits[node.sha] = rest;
    }

    return { rows, commits: publicCommits };
}

/** Left-to-right default: main, newest dated release branch, nightly. */
export function defaultBranches(refs) {
    const names = refs.knownBranches ?? [...refs.remoteBranches, ...refs.localBranches];
    const has = (name) => names.includes(name);
    const main = refs.defaultBranch ?? (has("main") ? "main" : null);
    const releases = names.filter((n) => n.startsWith("release/")).sort();
    const release = releases.filter((n) => /^release\/\d{4}-\d{2}/.test(n)).at(-1) ?? releases.at(-1);
    const picked = [main, release, has("nightly") ? "nightly" : null].filter(Boolean);
    return picked.length ? picked : names.slice(0, 1);
}
