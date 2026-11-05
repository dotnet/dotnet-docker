const PR_SUFFIX = /\(#\d+\)\s*$/;
const GITHUB_MERGE_SUBJECT = /^Merge pull request #(\d+) from \S+/;

// Branch-sync merges also carry a "(#N)" but bring in dozens of commits; real PRs here stay below this.
const MAX_PR_COMMITS = 10;

/** GitHub's default merge subject hides the PR title in the body; surface it. */
function withPrTitle(record) {
    const match = GITHUB_MERGE_SUBJECT.exec(record.subject);
    if (!match) {
        return record;
    }
    const title = record.body.split("\n").map((line) => line.trim()).find(Boolean);
    return title ? { ...record, subject: `${title} (#${match[1]})` } : record;
}

function reachableFrom(bySha, start) {
    const seen = new Set();
    const stack = [start];
    while (stack.length) {
        const sha = stack.pop();
        const record = bySha.get(sha);
        if (!record || seen.has(sha)) {
            continue;
        }
        seen.add(sha);
        stack.push(...record.parents);
    }
    return seen;
}

/** Commits on the PR side of a merge: reachable from `start` but not already in `base`. */
function pullRequestCommits(bySha, start, base, limit) {
    const found = new Set();
    const stack = [start];
    while (stack.length && found.size < limit) {
        const sha = stack.pop();
        const record = bySha.get(sha);
        if (!record || base.has(sha) || found.has(sha)) {
            continue;
        }
        found.add(sha);
        stack.push(...record.parents);
    }
    return found;
}

/**
 * Turns raw `git log` records (merges included) into the changes worth showing.
 *
 * A pull request merged with a merge commit appears as that one merge commit, under
 * the PR's title and number, and its own commits are hidden. Every other merge
 * (syncing branches, merging a release branch, anything touching more than
 * MAX_PR_COMMITS commits) is hidden and the commits it brought in are shown instead.
 */
export function selectChanges(records) {
    const all = records.map(withPrTitle);
    const bySha = new Map(all.map((record) => [record.sha, record]));
    const shownMerges = new Set();
    const hidden = new Set();

    for (const merge of all) {
        if (merge.parents.length !== 2 || !PR_SUFFIX.test(merge.subject)) {
            continue;
        }
        const [first, second] = merge.parents;
        if (!bySha.has(second)) {
            // The PR branch predates the loaded window, so its size is unknown.
            shownMerges.add(merge.sha);
            continue;
        }
        const inner = pullRequestCommits(bySha, second, reachableFrom(bySha, first), MAX_PR_COMMITS + 1);
        if (inner.size >= 1 && inner.size <= MAX_PR_COMMITS) {
            shownMerges.add(merge.sha);
            inner.forEach((sha) => hidden.add(sha));
        }
    }

    return all.filter((record) => (record.parents.length > 1 ? shownMerges.has(record.sha) : !hidden.has(record.sha)));
}
