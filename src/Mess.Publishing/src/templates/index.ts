import fs from "fs/promises";
import path from "path";

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
  pidgeon: {
    push: await importTemplate("pidgeon.push.json.hbs"),
  },
  abb: {
    push: await importTemplate("abb.push.json.hbs"),
  },
  schneider: {
    push: await importTemplate("schneider.push.json.hbs"),
  },
});

export default Template;
