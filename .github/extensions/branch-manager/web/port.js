import { escapeHtml, stripTitle } from "./util.js";

const $ = (selector) => document.querySelector(selector);

const branchKind = (name) =>
    name === "main" ? "main" : name === "nightly" ? "nightly" : name.startsWith("release/") ? "release" : "other";

// Where a change is most naturally ported from, by the kind of branch it is going to (see the porting-changes skill).
const PREFERRED_SOURCES = {
    release: ["nightly", "main", "release"],
    main: ["release", "nightly"],
    nightly: ["main", "release"],
};

function sourceRank(branches, target) {
    const preferred = PREFERRED_SOURCES[branchKind(target)] ?? [];
    return Math.min(
        ...branches.map((branch) => {
            const rank = preferred.indexOf(branchKind(branch));
            return rank === -1 ? preferred.length : rank;
        }),
    );
}

/**
 * The versions of a change that can be ported to `target`: one per branch that has merged commits for it,
 * with branches holding the identical commits (such as after a merge) offered as one option.
 * Open pull requests aren't offered because there is nothing merged to port yet.
 */
function sourceOptions(board, row, target) {
    const options = new Map();
    for (const { name } of board.branches) {
        if (name === target) {
            continue;
        }
        const shas = row.cells[name].filter((sha) => board.commits[sha].kind !== "pr");
        if (!shas.length) {
            continue;
        }
        const key = shas.join(",");
        const option = options.get(key) ?? { key, branches: [], shas };
        option.branches.push(name);
        options.set(key, option);
    }
    return [...options.values()].sort((a, b) => sourceRank(a.branches, target) - sourceRank(b.branches, target));
}

const portable = (board, row, branch) => !row.pending && row.cells[branch].length === 0 && sourceOptions(board, row, branch).length > 0;

/** Dialog for choosing what to port to a branch and reviewing the prompt before it goes to Copilot. */
export function createPortDialog({ api, toast, getBoard, getSelected, onSent }) {
    const dialog = $("#port-dialog");
    const targetSelect = $("#port-target");
    const list = $("#port-items");
    const prompt = $("#port-prompt");
    const sendButton = $("#port-send");
    let session = null;

    const included = () => session.items.filter((item) => item.choice.included);

    function showError(message) {
        const element = $("#port-error");
        element.textContent = message ?? "";
        element.hidden = !message;
    }

    function itemHtml(item, index) {
        const { commits } = session.board;
        const describe = (option) => {
            const changes = option.shas.map((sha) => commits[sha]);
            const refs = changes.map((change) => (change.pr !== null ? `#${change.pr}` : change.short)).join(", ");
            return `<span class="port-branch">${option.branches.map(escapeHtml).join(", ")}</span><span class="port-refs">${escapeHtml(refs)}</span><span class="port-text">${escapeHtml(stripTitle(changes[0].subject))}</span>`;
        };
        const chosen = item.options.find((option) => option.key === item.choice.key);

        const sources =
            item.options.length > 1
                ? `<div class="port-sources" role="radiogroup" aria-label="Port from">${item.options
                      .map(
                          (option) =>
                              `<label class="port-source"><input type="radio" name="source-${index}" data-role="source" data-i="${index}" data-key="${escapeHtml(option.key)}" ${option.key === item.choice.key ? "checked" : ""}>${describe(option)}</label>`,
                      )
                      .join("")}</div>`
                : `<div class="port-source single"><span class="muted">From</span>${describe(item.options[0])}</div>`;

        return `<li class="port-item" ${item.choice.included ? "" : "data-excluded"}>
          <label class="port-item-head"><input type="checkbox" data-role="include" data-i="${index}" ${item.choice.included ? "checked" : ""}><span class="port-title">${escapeHtml(stripTitle(session.board.commits[chosen.shas[0]].subject))}</span></label>
          ${sources}
        </li>`;
    }

    /** Rebuilds the list of changes for the current target, keeping earlier choices where they still apply. */
    function buildItems() {
        const { board, rows, target, choices } = session;
        session.items = [];
        let present = 0;
        for (const row of rows) {
            if (row.cells[target].length) {
                present++;
                continue;
            }
            const options = sourceOptions(board, row, target);
            if (!options.length) {
                continue;
            }
            const choice = choices.get(row.id) ?? { included: true, key: options[0].key };
            if (!options.some((option) => option.key === choice.key)) {
                choice.key = options[0].key;
            }
            choices.set(row.id, choice);
            session.items.push({ row, options, choice });
        }

        const notice = $("#port-notice");
        notice.hidden = present === 0;
        notice.textContent = present
            ? `${present} selected ${present === 1 ? "change is" : "changes are"} already on ${target} and ${present === 1 ? "was" : "were"} left out.`
            : "";
    }

    function renderItems() {
        const { target, items } = session;
        list.innerHTML = items.length
            ? items.map(itemHtml).join("")
            : `<li class="muted port-empty">Nothing to port to ${escapeHtml(target)}.</li>`;
    }

    function syncUi() {
        const count = included().length;
        $("#port-items-label").textContent = `Changes to port (${count} of ${session.items.length})`;
        $("#port-summary").textContent = count
            ? `${count} ${count === 1 ? "change" : "changes"} to ${session.target}`
            : "Choose at least one change to port.";
        $("#port-regenerate").hidden = !session.dirty;
        $("#port-restore").hidden = !session.backup || session.dirty;
        const note = $("#port-prompt-note");
        note.hidden = !session.stale;
        note.textContent = "The changes above were modified after you edited the prompt, so the prompt may be out of date.";
        sendButton.disabled = !count || session.generating || session.sending || !prompt.value.trim();
        sendButton.textContent = session.sending ? "Sending…" : "Send to Copilot";
    }

    /** What the generated prompt depends on, to tell whether a restored edit still matches the choices. */
    const signature = () =>
        JSON.stringify([session.target, included().map(({ row, choice }) => [row.id, choice.key])]);

    /** Rebuilds the prompt from the choices, keeping the user's hand-edited text so it can be restored. */
    function discardAndRegenerate() {
        if (session.dirty) {
            session.backup = { text: prompt.value, signature: signature() };
        }
        regenerate();
    }

    function restoreEdits() {
        const { text, signature: saved } = session.backup;
        session.sequence++;
        session.backup = null;
        session.generating = false;
        session.dirty = true;
        session.stale = saved !== signature();
        prompt.value = text;
        prompt.focus();
        syncUi();
    }

    /**
     * Every async continuation checks it still belongs to the dialog opening that started it (the session object)
     * and to the latest generation of that opening (the sequence), so late responses never touch newer state.
     * The previous prompt is cleared up front: it no longer matches the choices, so it must not be sendable if this fails.
     */
    async function regenerate() {
        const owner = session;
        const items = included();
        const sequence = ++owner.sequence;
        const current = () => session === owner && owner.sequence === sequence;
        owner.dirty = false;
        owner.stale = false;
        prompt.value = "";
        showError();

        if (!items.length) {
            owner.generating = false;
            syncUi();
            return;
        }

        owner.generating = true;
        syncUi();
        try {
            const { prompt: text } = await api("port/prompt", {
                target: owner.target,
                items: items.map(({ options, choice }) => {
                    const option = options.find((o) => o.key === choice.key);
                    return { shas: option.shas, branches: option.branches };
                }),
            });
            if (!current()) {
                return;
            }
            prompt.value = text;
        } catch (error) {
            if (current()) {
                showError(error.message);
            }
        }
        if (current()) {
            owner.generating = false;
            syncUi();
        }
    }

    /** The chosen changes or target changed: refresh the prompt unless the user has edited it by hand. */
    function changed() {
        if (session.dirty) {
            session.stale = true;
            syncUi();
        } else {
            regenerate();
        }
    }

    function open({ target, rowId } = {}) {
        const board = getBoard();
        if (!board) {
            return;
        }
        const selected = getSelected();
        const selectedRows = board.rows.filter((row) => row.shas.some((sha) => selected.has(sha)));
        const clicked = rowId ? board.rows.find((row) => row.id === rowId) : null;
        const rows = [...new Set([clicked, ...selectedRows].filter(Boolean))].filter((row) => !row.pending);
        if (!rows.length) {
            toast("Open pull requests can't be ported until they merge. Select a merged change instead.", "error");
            return;
        }

        const branches = board.branches.filter((branch) => !branch.error).map((branch) => branch.name);
        const missing = (branch) => rows.filter((row) => portable(board, row, branch)).length;
        const initial =
            target ?? branches.reduce((best, branch) => (missing(branch) > (best ? missing(best) : 0) ? branch : best), null);
        if (!initial) {
            toast("The selected changes are already on every tracked branch.", "error");
            return;
        }

        session = { board, rows, branches, target: initial, choices: new Map(), items: [], sequence: 0, dirty: false, stale: false, backup: null };
        targetSelect.innerHTML = branches
            .map((name) => `<option value="${escapeHtml(name)}" ${name === initial ? "selected" : ""}>${escapeHtml(name)}</option>`)
            .join("");
        $("#port-title").textContent = "Port changes";
        prompt.value = "";
        showError();
        buildItems();
        renderItems();
        syncUi();
        dialog.showModal();
        targetSelect.focus();
        regenerate();
    }

    function close() {
        if (dialog.open) {
            dialog.close();
        }
    }

    async function send() {
        if (sendButton.disabled) {
            return;
        }
        const owner = session;
        const count = included().length;
        const { target } = owner;
        owner.sending = true;
        showError();
        syncUi();
        try {
            await api("port/send", { prompt: prompt.value });
        } catch (error) {
            if (session === owner) {
                owner.sending = false;
                showError(error.message);
                syncUi();
            } else {
                toast(error.message, "error");
            }
            return;
        }
        // The prompt has gone to Copilot even if the dialog was closed meanwhile, but the page state may have moved on since.
        const active = session === owner;
        if (active) {
            close();
        }
        toast(`Sent to Copilot: port ${count} ${count === 1 ? "change" : "changes"} to ${target}`);
        if (active) {
            onSent();
        }
    }

    targetSelect.addEventListener("change", () => {
        session.target = targetSelect.value;
        buildItems();
        renderItems();
        changed();
        syncUi();
    });

    list.addEventListener("change", (event) => {
        const input = event.target.closest("[data-role]");
        if (!input) {
            return;
        }
        const item = session.items[Number(input.dataset.i)];
        if (input.dataset.role === "include") {
            item.choice.included = input.checked;
            input.closest(".port-item").toggleAttribute("data-excluded", !input.checked);
        } else {
            item.choice.key = input.dataset.key;
            const chosen = item.options.find((option) => option.key === item.choice.key);
            input.closest(".port-item").querySelector(".port-title").textContent = stripTitle(
                session.board.commits[chosen.shas[0]].subject,
            );
        }
        changed();
        syncUi();
    });

    prompt.addEventListener("input", () => {
        session.sequence++;
        session.generating = false;
        session.dirty = true;
        session.backup = null;
        syncUi();
    });
    $("#port-regenerate").addEventListener("click", discardAndRegenerate);
    $("#port-restore").addEventListener("click", restoreEdits);
    sendButton.addEventListener("click", send);
    $("#port-cancel").addEventListener("click", close);
    $("#port-close").addEventListener("click", close);
    dialog.addEventListener("close", () => {
        session = null;
    });

    return { open };
}
