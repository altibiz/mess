import { resolve, dirname } from "path";

const scriptLibDir = dirname(__filename);
export const scriptDir = resolve(scriptLibDir, "..").replace(/\\/g, "/");
export const rootDir = resolve(scriptDir, "..").replace(/\\/g, "/");
