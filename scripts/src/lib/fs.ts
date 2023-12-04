import fs from "fs";
import fsp from "fs/promises";
import * as g from "glob";
import p from "path";
import pp from "path/posix";
import { rootDir, scriptDir } from "./dirs";

export const script = (path?: fs.PathLike) => {
  return path
    ? p.isAbsolute(path.toString())
      ? path.toString()
      : pp.join(scriptDir, path.toString())
    : scriptDir;
};

export const root = (path?: fs.PathLike) => {
  return path
    ? p.isAbsolute(path.toString())
      ? path.toString()
      : pp.join(rootDir, path.toString())
    : rootDir;
};

export const exists = async (path: fs.PathLike) => {
  return fs.existsSync(root(path.toString()));
};

export const mkdir = async (path: fs.PathLike) => {
  return await fsp.mkdir(root(path.toString()), { recursive: true });
};

export const cp = async (src: fs.PathLike, dest: fs.PathLike) => {
  return await fsp.cp(root(src.toString()), root(dest.toString()));
};

export const mv = async (src: fs.PathLike, dest: fs.PathLike) => {
  const srcFull = root(src.toString());
  const destFull = root(dest.toString());
  if ((await fsp.stat(srcFull)).isDirectory()) {
    return await fsp.rename(srcFull, destFull);
  }

  let destIsDir = false;
  try {
    destIsDir = (await fsp.stat(destFull)).isDirectory();
  } catch (e) {
    destIsDir = false;
  }

  if (destIsDir) {
    return await fsp.rename(srcFull, pp.join(destFull, pp.basename(srcFull)));
  }

  return await fsp.rename(srcFull, destFull);
};

export const rmrf = async (path: fs.PathLike) => {
  const fullPath = root(path.toString());
  if (await exists(fullPath)) {
    return await fsp.rm(fullPath, { recursive: true, force: true });
  }
};

export const glob = async (pattern: string) => {
  return (await g.glob(root(pattern))).map((path) => path.replace(/\\/g, "/"));
};

export const globf = async (pattern: string, missing: string) => {
  const path = (await glob(pattern))[0];
  if (!path) {
    throw new Error(missing);
  }
  return path;
};

export const globi = async (pattern: string) =>
  Object.fromEntries(
    await Promise.all(
      (await glob(script(pattern))).map(async (commandPath) => {
        const name = pp.basename(commandPath, ".ts");
        const dir = pp.dirname(commandPath);
        return [name, await import(pp.join(dir, `${name}.js`))];
      }),
    ),
  );
