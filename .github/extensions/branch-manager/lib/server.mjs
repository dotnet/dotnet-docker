import http from "node:http";
import { randomBytes } from "node:crypto";
import { readFile } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";

const WEB_DIRECTORY = path.join(path.dirname(fileURLToPath(import.meta.url)), "..", "web");
const STATIC_FILES = {
    "": ["index.html", "text/html; charset=utf-8"],
    "index.html": ["index.html", "text/html; charset=utf-8"],
    "app.js": ["app.js", "text/javascript; charset=utf-8"],
    "port.js": ["port.js", "text/javascript; charset=utf-8"],
    "util.js": ["util.js", "text/javascript; charset=utf-8"],
    "styles.css": ["styles.css", "text/css; charset=utf-8"],
};
const MAX_BODY_BYTES = 1024 * 1024;

async function readJson(req) {
    const chunks = [];
    let size = 0;
    for await (const chunk of req) {
        size += chunk.length;
        if (size > MAX_BODY_BYTES) {
            throw Object.assign(new Error("Request body too large."), { status: 413 });
        }
        chunks.push(chunk);
    }
    if (!chunks.length) {
        return {};
    }
    try {
        return JSON.parse(Buffer.concat(chunks).toString("utf8"));
    } catch {
        throw Object.assign(new Error("Invalid JSON."), { status: 400 });
    }
}

/**
 * Loopback-only HTTP server for the canvas iframe. A random path token guards every
 * route so other local pages or processes can't drive the board.
 */
export async function startServer(service) {
    const token = randomBytes(16).toString("hex");
    const clients = new Set();
    let origin = "";

    const routes = {
        "GET api/board": (_body, query) => service.getBoard({ months: Number(query.get("months")) || 1 }),
        "POST api/refresh": () => service.refresh(),
        "POST api/link": (body) => service.link(body.shas ?? []),
        "POST api/unlink": (body) => service.unlink(body.shas ?? []),
        "POST api/config": (body) => service.setConfig(body),
        "POST api/port/prompt": (body) => service.composePortPrompt(body),
        "POST api/port/send": (body) => service.sendToCopilot(body.prompt),
        "POST api/open": (body) => service.openOnGitHub(body.url),
    };

    const server = http.createServer(async (req, res) => {
        const url = new URL(req.url, "http://127.0.0.1");
        const prefix = `/${token}/`;
        res.setHeader("Cache-Control", "no-store");
        res.setHeader("X-Content-Type-Options", "nosniff");

        const sendJson = (status, payload) => {
            res.writeHead(status, { "Content-Type": "application/json; charset=utf-8" });
            res.end(JSON.stringify(payload));
        };

        if (!url.pathname.startsWith(prefix) || (req.headers.origin && req.headers.origin !== origin)) {
            sendJson(404, { error: "Not found." });
            return;
        }
        const route = url.pathname.slice(prefix.length);

        try {
            if (req.method === "GET" && route in STATIC_FILES) {
                const [file, type] = STATIC_FILES[route];
                const content = await readFile(path.join(WEB_DIRECTORY, file));
                res.writeHead(200, { "Content-Type": type });
                res.end(content);
                return;
            }

            if (req.method === "GET" && route === "api/events") {
                res.writeHead(200, {
                    "Content-Type": "text/event-stream",
                    Connection: "keep-alive",
                });
                res.write("retry: 2000\n\n");
                clients.add(res);
                req.on("close", () => clients.delete(res));
                return;
            }

            const handler = routes[`${req.method} ${route}`];
            if (!handler) {
                sendJson(404, { error: "Not found." });
                return;
            }
            const body = req.method === "POST" ? await readJson(req) : {};
            sendJson(200, await handler(body, url.searchParams));
        } catch (error) {
            sendJson(error.status ?? 500, { error: error.message });
        }
    });

    await new Promise((resolve) => server.listen(0, "127.0.0.1", resolve));
    origin = `http://127.0.0.1:${server.address().port}`;

    const broadcast = () => clients.forEach((res) => res.write("event: changed\ndata: {}\n\n"));
    const unsubscribe = service.onChange(broadcast);
    const keepAlive = setInterval(() => clients.forEach((res) => res.write(": keep-alive\n\n")), 25_000);
    keepAlive.unref();

    return {
        url: `${origin}/${token}/`,
        async close() {
            clearInterval(keepAlive);
            unsubscribe();
            clients.forEach((res) => res.end());
            server.closeAllConnections();
            await new Promise((resolve) => server.close(resolve));
        },
    };
}
