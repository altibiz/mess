import { dirname, resolve } from "path";

const scriptSrcLibDir = dirname(__filename);
const scriptLibDir = resolve(scriptSrcLibDir, "..");

export const scriptDir = resolve(scriptLibDir, "..").replace(/\\/g, "/");
export const rootDir = resolve(scriptDir, "..").replace(/\\/g, "/");
