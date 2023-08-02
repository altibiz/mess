import * as g from "glob";
import p from "path/posix";
import fs from "fs";
import fsp from "fs/promises";
import { rootDir, scriptDir } from "./dirs";

export const script = (path: fs.PathLike) => {
  return p.isAbsolute(path.toString())
    ? path.toString()
    : p.join(scriptDir, path.toString());
};

export const root = (path: fs.PathLike) => {
  return p.isAbsolute(path.toString())
    ? path.toString()
    : p.join(rootDir, path.toString());
};

export const exists = async (path: fs.PathLike) => {
  return fs.existsSync(root(path.toString()));
};

export const mkdir = async (path: fs.PathLike) => {
  return await fsp.mkdir(root(path.toString()));
};

export const cp = async (src: fs.PathLike, dest: fs.PathLike) => {
  return await fsp.copyFile(root(src.toString()), root(dest.toString()));
};

export const rmrf = async (path: fs.PathLike) => {
  const fullPath = root(path.toString());
  if (await exists(fullPath)) {
    return await fsp.rm(fullPath, { recursive: true });
  }
};

export const glob = async (pattern: string) => {
  return await g.glob(root(pattern));
};

export const globf = async (pattern: string, missing: string) => {
  const path = (await g.glob(root(pattern)))[0];
  if (!path) {
    throw new Error(missing);
  }
  return path;
};

export const globi = async (pattern: string) =>
  Object.fromEntries(
    await Promise.all(
      (
        await glob(script(pattern))
      ).map(async (commandPath) => {
        const name = p.basename(commandPath, ".ts");
        const dir = p.dirname(commandPath);
        return [name, await import(p.join(dir, `${name}.js`))];
      }),
    ),
  );
