import { runCommand } from "./git.mjs";

const FIELDS = "number,title,author,createdAt,url,headRefName,isDraft,body";

/** Open pull requests targeting `branch`, via the GitHub CLI. */
export async function listOpenPullRequests(cwd, slug, branch) {
    const out = await runCommand(
        "gh",
        cwd,
        ["pr", "list", "--repo", slug, "--state", "open", "--base", branch, "--limit", "100", "--json", FIELDS],
        { timeout: 60_000 },
    );

    return JSON.parse(out).map((pr) => ({
        number: pr.number,
        title: pr.title,
        author: pr.author?.login ?? "unknown",
        createdAt: pr.createdAt,
        url: pr.url,
        headRef: pr.headRefName,
        isDraft: pr.isDraft,
        body: pr.body ?? "",
        base: branch,
    }));
}
