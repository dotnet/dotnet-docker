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

    const lines = [
        `Use the porting-changes skill to port the following ${many ? "changes" : "change"} to the \`${target}\` branch${repo}. ` +
            `Put ${many ? "all of them" : "it"} in a single new branch and a single pull request.`,
        "",
        many ? "Changes to port, oldest first:" : "Change to port:",
        ...items.flatMap(describeItem),
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
