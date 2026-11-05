import { spawn } from "node:child_process";

/** Opens a URL in the user's default browser (the canvas iframe can't open windows itself). */
export function openUrl(url) {
    const [command, args] =
        process.platform === "win32"
            ? ["cmd", ["/c", "start", "", url]]
            : process.platform === "darwin"
              ? ["open", [url]]
              : ["xdg-open", [url]];

    return new Promise((resolve, reject) => {
        const child = spawn(command, args, { detached: true, stdio: "ignore", windowsHide: true });
        child.once("error", reject);
        child.once("spawn", () => {
            child.unref();
            resolve();
        });
    });
}
