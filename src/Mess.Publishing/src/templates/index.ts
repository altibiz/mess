import path from "path";
import fs from "fs/promises";

type Template = {
  push?: string;
  update?: string;
};

const templatesPath = path.resolve(
  __dirname,
  "..",
  "..",
  "assets",
  "templates",
);

const importTemplate = async (name: string) =>
  (await fs.readFile(path.resolve(templatesPath, name))).toString();

export const importTemplates = async (): Promise<Record<string, Template>> => ({
  egauge: {
    push: await importTemplate("egauge.push.xml.hbs"),
  },
  eor: {
    push: await importTemplate("eor.push.json.hbs"),
    update: await importTemplate("eor.update.json.hbs"),
  },
});

export default Template;
