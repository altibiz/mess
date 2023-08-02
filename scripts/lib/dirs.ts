import { resolve, dirname } from "path";

const scriptLibDir = dirname(__filename);
export const scriptDir = resolve(scriptLibDir, "..");
export const rootDir = resolve(scriptDir, "..");
