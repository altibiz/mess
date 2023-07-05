import path from "path";
import * as glob from "glob";
import fs from "fs/promises";

export const importModulesDynamically = async <T>(
  dir: string,
): Promise<Record<string, T>> => {
  const messengerPaths = await glob.glob(`${dir}/*.ts`);
  const messengers = Object.fromEntries(
    (
      await Promise.all(
        messengerPaths.map(async (messengerPath) => {
          const id = path.parse(messengerPath).name;
          const messenger = await import(messengerPath);
          return [id, messenger];
        }),
      )
    ).filter(([, messenger]) => messenger.create),
  );

  return messengers;
};

export const importTextDynamically = async (
  dir: string,
): Promise<Record<string, string>> => {
  const templatePaths = await glob.glob(`${dir}/*.*`);
  const templates = Object.fromEntries(
    await Promise.all(
      templatePaths.map(async (templatePath) => {
        const id = path.parse(path.parse(templatePath).name).name;
        const buffer = await fs.readFile(templatePath);
        return [id, buffer.toString()];
      }),
    ),
  );

  return templates;
};
