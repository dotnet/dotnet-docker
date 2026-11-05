import { escapeHtml, octicon, setText, stripTitle } from "./util.js";
import { createPortDialog } from "./port.js";

const $ = (selector) => document.querySelector(selector);

const state = {
    board: null,
    months: 1,
    selected: new Set(),
    fetching: false,
    loading: false,
    error: null,
};

const ICONS = {
    plus: '<path d="M8 3v10M3 8h10"/>',
    close: '<path d="M4 4l8 8M12 4l-8 8"/>',
    // Sorted: one arrow, newest first. Idle: two arrows. The shapes differ so the state doesn't rely on color.
    sort: '<path d="M8 3v10M4 9l4 4 4-4"/>',
    sortIdle: '<path d="M5 13V3M2.5 5.5L5 3l2.5 2.5M11 3v10M8.5 10.5L11 13l2.5-2.5"/>',
    chevron: '<path d="M4 6l4 4 4-4"/>',
    unlink: '<path d="M6.5 9.5l3-3M5 8l-1.5 1.5a2.1 2.1 0 003 3L8 11M11 8l1.5-1.5a2.1 2.1 0 00-3-3L8 5"/>',
    commit: '<circle cx="8" cy="8" r="3"/><path d="M1 8h4M11 8h4"/>',
};

const icon = (name) =>
    `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">${ICONS[name]}</svg>`;

async function api(route, body) {
    let response;
    try {
        response = await fetch(
            `api/${route}`,
            body === undefined
                ? {}
                : { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(body) },
        );
    } catch {
        throw new Error("Lost connection to the Branch manager extension. Close and reopen this canvas.");
    }
    const data = await response.json().catch(() => ({}));
    if (!response.ok) {
        throw new Error(data.error || response.statusText);
    }
    return data;
}

const dayFormat = new Intl.DateTimeFormat(undefined, { weekday: "long", year: "numeric", month: "short", day: "numeric" });
const dateTimeFormat = new Intl.DateTimeFormat(undefined, {
    weekday: "long",
    year: "numeric",
    month: "short",
    day: "numeric",
    hour: "numeric",
    minute: "2-digit",
});
const relativeFormat = new Intl.RelativeTimeFormat(undefined, { numeric: "auto" });

function relativeTime(iso) {
    const seconds = (Date.parse(iso) - Date.now()) / 1000;
    const units = [
        ["year", 31536000],
        ["month", 2592000],
        ["day", 86400],
        ["hour", 3600],
        ["minute", 60],
    ];
    for (const [unit, size] of units) {
        if (Math.abs(seconds) >= size) {
            return relativeFormat.format(Math.round(seconds / size), unit);
        }
    }
    return "just now";
}

const infoToast = $("#toast");
const errorToast = $("#toast-error");
let toastTimer;

/**
 * Both toast regions stay in the page, empty, so screen readers register them before any text arrives.
 * Info toasts clear themselves; errors stay until dismissed because they may need acting on.
 */
function toast(message, kind = "info") {
    if (kind === "error") {
        const text = document.createElement("span");
        text.textContent = message;
        const dismiss = document.createElement("button");
        dismiss.type = "button";
        dismiss.className = "text-btn";
        dismiss.textContent = "Dismiss";
        dismiss.addEventListener("click", dismissError);
        errorToast.replaceChildren(text, dismiss);
        return;
    }
    infoToast.textContent = message;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => infoToast.replaceChildren(), 5000);
}

function dismissError() {
    errorToast.replaceChildren();
}

async function run(action, { success } = {}) {
    try {
        const result = await action();
        if (success) {
            toast(success);
        }
        return result;
    } catch (error) {
        toast(error.message, "error");
        return undefined;
    }
}

let loadInFlight = null;
let loadQueued = false;

async function load() {
    if (loadInFlight) {
        loadQueued = true;
        return loadInFlight;
    }
    state.loading = true;
    renderToolbar();
    loadInFlight = (async () => {
        try {
            state.board = await api(`board?months=${state.months}`);
            state.error = null;
            const known = new Set(Object.keys(state.board.commits));
            state.selected = new Set([...state.selected].filter((sha) => known.has(sha)));
        } catch (error) {
            state.error = error.message;
        } finally {
            state.loading = false;
            loadInFlight = null;
            render();
            if (loadQueued) {
                loadQueued = false;
                load();
            }
        }
    })();
    return loadInFlight;
}

async function refresh() {
    if (state.fetching) {
        return;
    }
    state.fetching = true;
    renderToolbar();
    const result = await run(() => api("refresh", {}));
    state.fetching = false;
    if (result?.failed?.length) {
        const names = result.failed.map((f) => f.branch).join(", ");
        const repo = state.board?.repo.slug ?? "the repository";
        toast(`Couldn't fetch ${names}. Check your network connection and your access to ${repo}, then select Refresh.`, "error");
    }
    await load();
}

/** First line of a failure message, which is usually all a person needs to read. */
const firstLine = (text) => String(text).split("\n").find((line) => line.trim())?.trim().replace(/[.:]$/, "") ?? "";

function renderToolbar() {
    const board = state.board;
    setText($("#repo-name"), board?.repo.slug ?? "");

    // aria-disabled, not disabled, so keyboard focus stays on the button while it works.
    const refreshButton = $("#refresh");
    refreshButton.setAttribute("aria-disabled", String(state.fetching));

    const pullRequestError = board?.openPullRequests?.error;
    const parts = state.fetching
        ? ["Fetching branches…"]
        : [
              board?.fetchedAt ? `Fetched ${relativeTime(board.fetchedAt)}` : "",
              pullRequestError
                  ? `Open pull requests unavailable: ${firstLine(pullRequestError)}. Check that the GitHub CLI (gh) is installed and signed in, then select Refresh.`
                  : "",
          ];
    setText($("#status"), parts.filter(Boolean).join(" · "));

    const count = state.selected.size;
    $("#link").disabled = count < 2;
    setText($("#link"), count >= 2 ? `Link changes (${count})` : "Link changes");

    const unlinkable = selectedLinked();
    $("#unlink").disabled = unlinkable.length === 0;
    setText($("#unlink"), unlinkable.length ? `Unlink (${unlinkable.length})` : "Unlink");

    const portable = selectedRowCount();
    $("#port").disabled = portable === 0;
    setText($("#port"), portable ? `Port ${portable} ${portable === 1 ? "change" : "changes"} to…` : "Port to…");

    // Text beside the buttons, because a tooltip on a disabled button never reaches keyboard users.
    setText(
        $("#hint"),
        !board ? "" : count === 0 ? "Tick changes to link, unlink or port them." : count === 1 ? "1 selected. Tick another to link them." : `${count} selected`,
    );
}

/** Number of distinct changes (rows) that have at least one selected commit or pull request. */
function selectedRowCount() {
    if (!state.board || !state.selected.size) {
        return 0;
    }
    return state.board.rows.filter((row) => row.shas.some((sha) => state.selected.has(sha))).length;
}

/** Selected commits that are currently part of a linked row (manual or automatic). */
function selectedLinked() {
    if (!state.board) {
        return [];
    }
    const linked = new Set(state.board.rows.filter((row) => row.linked.length).flatMap((row) => row.shas));
    return [...state.selected].filter((sha) => linked.has(sha));
}

function reasonLabel(reasons) {
    if (reasons.includes("manual")) {
        return ["Linked", "Linked manually"];
    }
    const [reason] = reasons;
    return {
        "cherry-pick": ["Auto: cherry-pick", "Matched by a cherry-pick trailer"],
        reference: ["Auto: reference", "Matched by a port/backport reference to another PR"],
        title: ["Auto: same title", "Matched by identical title on different branches"],
    }[reason];
}

function cardHtml(sha, row) {
    const commit = state.board.commits[sha];
    const isPr = commit.kind === "pr";
    const linked = row.linked.length > 0;
    const selected = state.selected.has(sha);
    const pr = !isPr && commit.pr !== null ? `#${commit.pr}` : "";
    const title = isPr ? commit.subject : commit.subject.replace(/\s*\(#\d+\)\s*$/, "");
    const what = isPr ? "pull request" : "commit";

    // Opened by the extension (the canvas iframe can't open new windows), hence data-open.
    const link = (url, className, text, label) =>
        url ? `<a class="${className}" href="${escapeHtml(url)}" data-open title="${label}">${text}</a>` : text;

    const titleHtml = link(commit.url, "commit-link", escapeHtml(title), `Open ${what} on GitHub`);
    const prIcon = isPr
        ? octicon(commit.isDraft ? "git-pull-request-draft" : "git-pull-request", `pr-icon ${commit.isDraft ? "draft" : "open"}`)
        : "";
    const prHtml = pr ? ` ${link(commit.prUrl, "pr", `(${pr})`, "Open pull request on GitHub")}` : "";
    const shaHtml = link(commit.url, "sha", commit.short, `Open ${what} on GitHub`);
    const stateChip = isPr && commit.isDraft ? `<span class="chip draft" title="Draft pull request">Draft</span>` : "";
    const when = isPr ? `opened ${relativeTime(commit.date)}` : relativeTime(commit.date);

    const [label, tooltip] = linked ? reasonLabel(row.linked) : [];
    const chip = linked ? `<span class="chip linked" title="${escapeHtml(tooltip)}">${escapeHtml(label)}</span>` : "";
    const unlink = linked
        ? `<button type="button" class="mini-btn" data-action="unlink" data-sha="${sha}" data-focus="unlink:${sha}" title="Not the same change: separate this ${what} from the others in this row" aria-label="Unlink ${commit.short} from this row">${icon("unlink")}Unlink</button>`
        : "";

    return `
      <article class="card" data-sha="${sha}" ${isPr ? 'data-kind="pr"' : ""} ${selected ? "data-selected" : ""}>
        <input type="checkbox" data-action="select" data-sha="${sha}" data-focus="select:${sha}" ${selected ? "checked" : ""} aria-label="Select ${escapeHtml(commit.short)} ${escapeHtml(title)}">
        <div>
          <div class="card-title">${prIcon}<span>${titleHtml}${prHtml}</span></div>
          <div class="meta">
            ${shaHtml}
            <span>${escapeHtml(commit.author)}</span>
            <time datetime="${escapeHtml(commit.date)}" title="${escapeHtml(dateTimeFormat.format(new Date(commit.date)))}">${escapeHtml(when)}</time>
            ${stateChip}
            ${chip}
            ${unlink}
          </div>
        </div>
      </article>`;
}

function cellHtml(row, branch, errored) {
    const shas = row.cells[branch.name];
    const name = escapeHtml(branch.name);
    // Each cell names its branch, because a screen reader reads the rows without the column headers.
    const open = `<div class="cell" role="group" aria-label="${name}">`;
    if (shas.length > 1) {
        // Several commits that are one change share a single outer border.
        return `${open}<div class="stack">${shas.map((sha) => cardHtml(sha, row)).join("")}</div></div>`;
    }
    if (shas.length) {
        return `${open}${cardHtml(shas[0], row)}</div>`;
    }
    if (errored || row.pending) {
        return `${open}</div>`;
    }
    const subject = escapeHtml(stripTitle(state.board.commits[row.shas[0]].subject));
    return `${open}<button type="button" class="gap" data-action="port" data-row="${row.id}" data-branch="${name}" data-focus="port:${row.id}:${name}" title="Port this change to ${name}" aria-label="Port “${subject}” to ${name}">${icon("plus")}</button></div>`;
}

function headHtml(branch, board) {
    const name = escapeHtml(branch.name);
    const sorted = board.sortBranch === branch.name;
    return `
      <div class="col-head" data-branch="${name}">
        <button type="button" class="name-btn" data-action="pick-branch" data-branch="${name}" data-focus="pick:${name}" title="Switch branch" aria-haspopup="dialog" aria-expanded="false" aria-label="${name}: switch branch">
          <span class="name">${name}</span>${icon("chevron")}
        </button>
        <span class="count" title="Changes loaded for this branch">${branch.count}</span>
        <button type="button" class="icon-btn" data-action="sort" data-branch="${name}" data-focus="sort:${name}" aria-pressed="${sorted}" title="${sorted ? "Rows are sorted by this branch, newest first" : `Sort rows by ${name}, newest first`}" aria-label="Sort by ${name}">${icon(sorted ? "sort" : "sortIdle")}</button>
        <button type="button" class="icon-btn" data-action="remove-branch" data-branch="${name}" data-focus="remove:${name}" title="Stop tracking ${name}" aria-label="Stop tracking ${name}">${icon("close")}</button>
        ${branch.error ? `<span class="warn">${escapeHtml(branch.error)}</span>` : ""}
      </div>`;
}

function renderBoard() {
    const board = state.board;
    const element = $("#board");

    if (!board) {
        element.style.removeProperty("--cols");
        element.innerHTML = state.error
            ? `<div class="board-error">${escapeHtml(state.error)}</div>`
            : `<div class="loading">Loading…</div>`;
        return;
    }

    element.style.setProperty("--cols", board.branches.length);
    const parts = [
        `<div class="lines" aria-hidden="true">${board.branches.map(() => "<i></i>").join("")}</div>`,
        `<div class="head-row">${board.branches.map((branch) => headHtml(branch, board)).join("")}</div>`,
    ];

    if (!board.rows.length) {
        parts.push(`<div class="empty">No changes in this time range.</div>`);
    }

    let section = null;
    for (const row of board.rows) {
        if (row.open) {
            if (section !== "open") {
                parts.push(`<div class="day" role="heading" aria-level="2"><span>${octicon("git-pull-request")}</span><span>Open pull requests</span></div>`);
                section = "open";
            }
        } else {
            const day = dayFormat.format(new Date(row.sortTime));
            if (day !== section) {
                parts.push(`<div class="day" role="heading" aria-level="2"><span>${icon("commit")}</span><span>Changes on ${escapeHtml(day)}</span></div>`);
                section = day;
            }
        }
        const cells = board.branches.map((branch) => cellHtml(row, branch, Boolean(branch.error))).join("");
        parts.push(`<div class="row">${cells}</div>`);
    }

    element.innerHTML = parts.join("");
}

function renderFooter() {
    const board = state.board;
    const footer = $("#footer");
    if (!board) {
        footer.innerHTML = "";
        return;
    }
    const since = dayFormat.format(new Date(board.since));
    footer.innerHTML = board.hasMore
        ? `<span>Showing changes since ${escapeHtml(since)}</span><button type="button" class="btn" data-action="load-more" data-focus="more" ${state.loading ? "disabled" : ""}>Load older changes</button>`
        : `<span>Showing all changes since ${escapeHtml(since)}. There is nothing older.</span>`;
}

// The board is rebuilt on every change, which would drop keyboard focus onto <body>.
// Remember the focused control by its data-focus key and put focus back afterwards.
let focusRequest = null;

function focusedKey() {
    const element = document.activeElement;
    return element?.dataset?.focus && element.closest("#board, #footer") ? element.dataset.focus : null;
}

function restoreFocus(keys) {
    for (const key of keys) {
        const target = document.querySelector(`[data-focus="${CSS.escape(key)}"]`);
        if (target) {
            target.focus();
            return;
        }
    }
    // The control is gone, so land on the nearest stable place instead of <body>.
    (keys.includes("more") ? $("#footer") : document.querySelector('[data-focus^="pick:"]'))?.focus();
}

function render() {
    const keys = focusRequest ?? [focusedKey()].filter(Boolean);
    focusRequest = null;
    renderToolbar();
    renderBoard();
    renderFooter();
    updateHeadHeight();
    if (keys.length) {
        restoreFocus(keys);
    }
}

// Focused controls scroll into view; keep them clear of the sticky column header (WCAG 2.4.11).
function updateHeadHeight() {
    const head = document.querySelector(".head-row");
    if (head) {
        $("#scroller").style.setProperty("--head-height", `${head.offsetHeight}px`);
    }
}
new ResizeObserver(updateHeadHeight).observe($("#board"));

function updateSelectionUi(sha) {
    document.querySelectorAll(`.card[data-sha="${sha}"]`).forEach((card) => {
        card.toggleAttribute("data-selected", state.selected.has(sha));
        card.querySelector("input[type=checkbox]").checked = state.selected.has(sha);
    });
    renderToolbar();
}

function trackedBranches() {
    return state.board.branches.map((b) => b.name);
}

$("#board").addEventListener("change", (event) => {
    const input = event.target.closest('[data-action="select"]');
    if (!input) {
        return;
    }
    const sha = input.dataset.sha;
    if (input.checked) {
        state.selected.add(sha);
    } else {
        state.selected.delete(sha);
    }
    updateSelectionUi(sha);
});

$("#board").addEventListener("click", (event) => {
    const link = event.target.closest("a[data-open]");
    if (link) {
        event.preventDefault();
        run(() => api("open", { url: link.href }), { success: "Opened in your browser" });
    }
});

$("#board").addEventListener("click", async (event) => {
    const button = event.target.closest("button[data-action]");
    if (!button) {
        return;
    }
    const { action, branch, sha, row: rowId } = button.dataset;

    if (action === "sort") {
        await run(() => api("config", { sortBranch: branch }));
        await load();
    } else if (action === "remove-branch") {
        const remaining = trackedBranches().filter((name) => name !== branch);
        if (!remaining.length) {
            toast("Track at least one branch.", "error");
            return;
        }
        await run(() => api("config", { branches: remaining }));
        await load();
    } else if (action === "unlink") {
        // The Unlink button disappears with the link, so hand focus to the same commit's checkbox.
        focusRequest = [`select:${sha}`];
        await run(() => api("unlink", { shas: [sha] }), { success: "Unlinked" });
        state.selected.delete(sha);
        await load();
    } else if (action === "port") {
        portDialog.open({ target: branch, rowId });
    }
});

$("#footer").addEventListener("click", async (event) => {
    if (event.target.closest('[data-action="load-more"]')) {
        const previousRows = state.board.rows.length;
        state.months += 1;
        await load();
        // Move focus into the new content instead of leaving it on a button that is now far below it.
        const firstNew = document.querySelectorAll("#board .row")[previousRows];
        (firstNew?.querySelector("[data-focus^='select:']") ?? document.querySelector('[data-focus="more"]') ?? $("#footer")).focus();
        toast(`Loaded changes back to ${dayFormat.format(new Date(state.board.since))}.`);
    }
});

$("#link").addEventListener("click", async () => {
    const shas = [...state.selected];
    focusRequest = [`select:${shas[0]}`];
    const result = await run(() => api("link", { shas }), { success: `Linked ${shas.length} changes` });
    if (result) {
        state.selected.clear();
        await load();
    } else {
        focusRequest = null;
    }
});

$("#unlink").addEventListener("click", async () => {
    const shas = selectedLinked();
    focusRequest = [`select:${shas[0]}`];
    const result = await run(() => api("unlink", { shas }), { success: `Unlinked ${shas.length} ${shas.length === 1 ? "change" : "changes"}` });
    if (result) {
        shas.forEach((sha) => state.selected.delete(sha));
        await load();
    } else {
        focusRequest = null;
    }
});

$("#refresh").addEventListener("click", refresh);

$("#port").addEventListener("click", () => portDialog.open({}));

const portDialog = createPortDialog({
    api,
    run,
    toast,
    getBoard: () => state.board,
    getSelected: () => state.selected,
    onSent: () => {
        // The toolbar button that opened the dialog is disabled once the selection clears.
        const first = [...state.selected][0];
        state.selected.clear();
        focusRequest = first ? [`select:${first}`] : null;
        render();
    },
});

const popover = $("#branch-popover");
const addButton = $("#add-branch");
const filterInput = $("#branch-filter");

// mode "add" appends a column; mode "replace" swaps `branch` for the chosen one.
let picker = null;

function branchOrder(a, b) {
    const rank = (name) => (name === "main" ? 0 : name === "nightly" ? 1 : name.startsWith("release/") ? 2 : 3);
    return rank(a) - rank(b) || (rank(a) === 2 ? b.localeCompare(a) : a.localeCompare(b));
}

function renderBranchList() {
    const filter = $("#branch-filter").value.trim().toLowerCase();
    const tracked = new Set(trackedBranches());
    const matches = state.board.availableBranches
        .filter((name) => !tracked.has(name) && name.toLowerCase().includes(filter))
        .sort(branchOrder);
    const shown = matches.slice(0, 50);

    $("#branch-list").innerHTML =
        (shown.length
            ? shown.map((name) => `<li><button type="button" data-branch="${escapeHtml(name)}">${escapeHtml(name)}</button></li>`).join("")
            : `<li class="hint">No matching branches.</li>`) +
        (matches.length > shown.length ? `<li class="hint">${matches.length - shown.length} more. Keep typing to narrow.</li>` : "");
    setText($("#branch-count"), `${matches.length} ${matches.length === 1 ? "branch" : "branches"}`);
}

function positionPopover(trigger, mode) {
    const margin = 8;
    const rect = trigger.getBoundingClientRect();
    const width = popover.offsetWidth;
    const left = mode === "add" ? rect.right - width : rect.left;
    popover.style.left = `${Math.max(margin, Math.min(left, window.innerWidth - width - margin))}px`;
    popover.style.top = `${rect.bottom + 6}px`;
}

function closePicker({ restoreFocus = false } = {}) {
    if (!picker) {
        return;
    }
    const { mode, branch, trigger } = picker;
    picker = null;
    popover.hidden = true;
    trigger.setAttribute("aria-expanded", "false");
    if (restoreFocus) {
        const target = mode === "add" ? addButton : [...document.querySelectorAll(".name-btn")].find((b) => b.dataset.branch === branch);
        target?.focus();
    }
}

function openPicker(mode, trigger, branch) {
    if (!state.board) {
        return;
    }
    if (picker && picker.trigger === trigger) {
        closePicker();
        return;
    }
    closePicker();
    picker = { mode, branch, trigger };
    $("#picker-title").textContent = mode === "add" ? "Track another branch" : `Replace ${branch} with`;
    popover.hidden = false;
    trigger.setAttribute("aria-expanded", "true");
    filterInput.value = "";
    renderBranchList();
    positionPopover(trigger, mode);
    filterInput.focus();
}

async function chooseBranch(name) {
    const { mode, branch } = picker;
    closePicker();
    const tracked = trackedBranches();
    const config = { branches: mode === "replace" ? tracked.map((b) => (b === branch ? name : b)) : [...tracked, name] };
    if (mode === "replace" && state.board.sortBranch === branch) {
        config.sortBranch = name;
    }
    // The column's header button is rebuilt under the new name; the toolbar button just takes focus back.
    focusRequest = mode === "replace" ? [`pick:${name}`] : null;
    if (mode === "add") {
        addButton.focus();
    }
    await run(() => api("config", config));
    await load();
    refresh();
}

addButton.addEventListener("click", () => openPicker("add", addButton));
$("#board").addEventListener("click", (event) => {
    const trigger = event.target.closest('button[data-action="pick-branch"]');
    if (trigger) {
        openPicker("replace", trigger, trigger.dataset.branch);
    }
});
filterInput.addEventListener("input", renderBranchList);
filterInput.addEventListener("keydown", (event) => {
    const first = $("#branch-list button");
    if (event.key === "Enter" && first) {
        event.preventDefault();
        chooseBranch(first.dataset.branch);
    } else if (event.key === "ArrowDown" && first) {
        event.preventDefault();
        first.focus();
    }
});
$("#branch-list").addEventListener("click", (event) => {
    const button = event.target.closest("button[data-branch]");
    if (button) {
        chooseBranch(button.dataset.branch);
    }
});
$("#branch-list").addEventListener("keydown", (event) => {
    if (event.key !== "ArrowDown" && event.key !== "ArrowUp") {
        return;
    }
    event.preventDefault();
    const items = [...document.querySelectorAll("#branch-list button")];
    const next = items.indexOf(document.activeElement) + (event.key === "ArrowDown" ? 1 : -1);
    (next < 0 ? filterInput : items[Math.min(next, items.length - 1)]).focus();
});
document.addEventListener("click", (event) => {
    if (picker && !popover.contains(event.target) && !event.target.closest('#add-branch, [data-action="pick-branch"]')) {
        closePicker();
    }
});
document.addEventListener("keydown", (event) => {
    if (event.key !== "Escape") {
        return;
    }
    if (picker) {
        closePicker({ restoreFocus: true });
    } else if (errorToast.hasChildNodes() && !$("#port-dialog").open) {
        dismissError();
    }
});
// Tabbing out of the picker closes it, so it never stays open behind focus that has moved on.
popover.addEventListener("focusout", (event) => {
    const next = event.relatedTarget;
    if (picker && next !== null && !popover.contains(next) && next !== picker.trigger) {
        closePicker();
    }
});
$("#scroller").addEventListener("scroll", () => closePicker(), { passive: true });
window.addEventListener("resize", () => closePicker());

const events = new EventSource("api/events");
let changeTimer;
events.addEventListener("changed", () => {
    clearTimeout(changeTimer);
    changeTimer = setTimeout(load, 200);
});

render();
await load();
refresh();
