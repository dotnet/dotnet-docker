/**
 * Builds the prompt for porting one or more changes to a branch, as a single branch and PR.
 * The user reviews and can edit this text before it is sent, and the wording is easy to tune here.
 *
 * `items` are the changes to port, oldest first. Each has the branches it comes from and its
 * commits (oldest first); a single change can span several commits.
 */
export function buildPortPrompt({ repoSlug, target, items }) {
    const many = items.length > 1;
    const repo = repoSlug ? ` in ${repoSlug}` : "";
    const sources = [...new Set(items.flatMap((item) => item.branches))];
    const title = many
        ? `Port changes from ${sources.length ? sources.join(", ") : "other branches"} to ${target}`
        : `[${target}] ${stripTitle(items[0].changes.at(-1).subject)}`;

    const lines = [
        `Port the following ${many ? "changes" : "change"} to the \`${target}\` branch${repo}. ` +
            `Put ${many ? "all of them" : "it"} in a single new branch and a single pull request.`,
        "",
        many ? "Changes to port, oldest first:" : "Change to port:",
        ...items.flatMap(describeItem),
        "",
        "Instructions:",
        "- Use the porting-changes skill and its pull request template.",
        `- Base the new branch on \`origin/${target}\`. Cherry-pick the final commits listed above, in order, ` +
            "and resolve any conflicts so each change keeps its original intent.",
        `- Pull request title: \`${title}\``,
        '- In the pull request description, link every original pull request listed above (for example "Ports #1234") ' +
            "so the Branch manager can associate this pull request with the originals.",
        ...(many ? ["- Since this pull request contains multiple changes, it should be rebase merged."] : []),
        "- Show me the result and get my confirmation before opening the pull request.",
    ];
    return lines.join("\n");
}

const stripTitle = (subject) =>
    subject
        .replace(/\s*\(#\d+\)\s*$/, "")
        .replace(/^(?:\[[^\]]+\]\s*)+/, "")
        .trim();

function describeChange(change) {
    if (change.kind === "pr") {
        return `Open PR #${change.pr} ${change.subject} - ${change.url}`;
    }
    return `\`${change.short}\` ${change.subject}${change.prUrl ? ` - ${change.prUrl}` : ""}`;
}

function describeItem(item) {
    const from = item.branches.length ? ` (from ${item.branches.map((b) => `\`${b}\``).join(", ")})` : "";
    if (item.changes.length === 1) {
        return [`- ${describeChange(item.changes[0])}${from}`];
    }
    return [
        `- ${stripTitle(item.changes.at(-1).subject)}${from}, ${item.changes.length} commits:`,
        ...item.changes.map((change) => `  - ${describeChange(change)}`),
    ];
}
