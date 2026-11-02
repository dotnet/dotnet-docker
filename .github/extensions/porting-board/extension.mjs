import { dirname } from "node:path";
import { fileURLToPath } from "node:url";
import { CanvasError, createCanvas, joinSession } from "@github/copilot-sdk/extension";
import { createService } from "./lib/service.mjs";
import { startServer } from "./lib/server.mjs";

let session;
const service = createService({
    // The extension lives inside the repo it manages, so its own folder is a reliable fallback.
    getWorkingDirectories: () => [process.cwd(), dirname(fileURLToPath(import.meta.url))],
    sendPrompt: (prompt) => session.send({ prompt }),
});

let server = null;
const openInstances = new Set();

// Wraps an action so failures reach the agent as structured canvas errors.
const action = (name, description, inputSchema, run) => ({
    name,
    description,
    inputSchema,
    handler: async (ctx) => {
        try {
            return await run(ctx.input ?? {});
        } catch (error) {
            throw new CanvasError("porting_board_error", error.message);
        }
    },
});

const changeId = { type: "string", pattern: "^([0-9a-fA-F]{7,40}|pr:\\d+)$" };

const shasSchema = {
    type: "array",
    items: changeId,
    minItems: 2,
    description:
        "Commit SHAs (full or abbreviated), or `pr:<number>` for an open pull request, that are the same logical change.",
};

const portingBoard = createCanvas({
    id: "porting-board",
    displayName: "Branch manager",
    description:
        "Side-by-side commit and open pull request history for several branches, with changes linked across branches and gaps shown where a change is missing.",
    inputSchema: {
        type: "object",
        properties: {
            branches: { type: "array", items: { type: "string" }, description: "Branches to track, in column order." },
            sortBranch: { type: "string", description: "Branch whose history orders the rows." },
        },
        additionalProperties: false,
    },
    actions: [
        action(
            "get_board",
            "Summarize recent changes per branch: which commits are linked across branches and which branches are missing each change.",
            {
                type: "object",
                properties: {
                    months: { type: "integer", minimum: 1, maximum: 60, description: "How many months of history to load. Default 1." },
                    onlyMissing: { type: "boolean", description: "Only include rows missing from at least one tracked branch." },
                    limit: { type: "integer", minimum: 1, maximum: 500, description: "Maximum rows to return. Default 100." },
                },
                additionalProperties: false,
            },
            (input) => service.describeBoard(input),
        ),
        action(
            "link_changes",
            "Record that several commits across branches are the same change. Links persist across sessions and worktrees.",
            {
                type: "object",
                properties: { shas: shasSchema },
                required: ["shas"],
                additionalProperties: false,
            },
            (input) => service.link(input.shas, { by: "copilot" }),
        ),
        action(
            "unlink_changes",
            "Separate commits from the change they were linked to, including automatic matches.",
            {
                type: "object",
                properties: {
                    shas: {
                        type: "array",
                        items: changeId,
                        minItems: 1,
                    },
                },
                required: ["shas"],
                additionalProperties: false,
            },
            (input) => service.unlink(input.shas),
        ),
        action(
            "set_branches",
            "Choose which branches are tracked (column order) and which one sorts the rows.",
            {
                type: "object",
                properties: {
                    branches: { type: "array", items: { type: "string" }, minItems: 1 },
                    sortBranch: { type: "string" },
                },
                additionalProperties: false,
            },
            (input) => service.setConfig(input),
        ),
        action(
            "refresh",
            "Fetch the latest history for the tracked branches from the remote.",
            { type: "object", additionalProperties: false },
            () => service.refresh(),
        ),
    ],
    open: async (ctx) => {
        if (ctx.input?.branches || ctx.input?.sortBranch) {
            await service.setConfig(ctx.input);
        }
        server ??= await startServer(service);
        openInstances.add(ctx.instanceId);
        return { url: server.url, title: "Branch manager" };
    },
    onClose: async (ctx) => {
        openInstances.delete(ctx.instanceId);
        if (openInstances.size === 0 && server) {
            const closing = server;
            server = null;
            await closing.close();
        }
    },
});

// `gh` lists open pull requests and needs the user's token, which the CLI withholds from
// extensions unless they ask for it by name. If the user declines, the board still works
// and just can't show open pull requests.
try {
    session = await joinSession({ canvases: [portingBoard], requestedEnvironmentVariables: ["GH_TOKEN"] });
} catch {
    session = await joinSession({ canvases: [portingBoard] });
}
