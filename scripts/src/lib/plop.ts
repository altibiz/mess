import fs from "fs/promises";
import handlebars from "handlebars";
import path from "path/posix";
import { root } from "./fs";

export const plopf = async (
  template: string,
  destination: string,
  config: Record<string, unknown>,
) => {
  const templatePath = root(template);
  const destinationPath = root(destination);
  await fs.mkdir(destinationPath, { recursive: true });

  const name = path.basename(templatePath);
  const content = await fs.readFile(template, "utf8");

  const nameTemplate = handlebars.compile(name.replace(/\.hbs$/, ""));
  const resultName = nameTemplate(config);
  const resultPath = path.join(destinationPath, resultName);

  const contentTemplate = handlebars.compile(content);
  const resultContent = contentTemplate(config);

  await fs.writeFile(resultPath, resultContent, "utf8");
};

export const plopd = async (
  template: string,
  destination: string,
  config: Record<string, unknown>,
) => {
  const templatePath = root(`scripts/assets/plop/${template}`);
  const destinationPath = root(destination);

  const content = await fs.readdir(templatePath);
  for (const name of content) {
    const namePath = path.join(templatePath, name);
    if ((await fs.stat(namePath)).isDirectory()) {
      const nameTemplate = handlebars.compile(name);
      const resultName = nameTemplate(config);
      const resultPath = path.join(destinationPath, resultName);

      await fs.mkdir(resultPath, { recursive: true });

      // NOTE: a bit backwards, but we need to pass the relative path
      const dirTemplate = path.relative(root("scripts/assets/plop"), namePath);
      await plopd(dirTemplate, resultPath, config);
    } else {
      await plopf(namePath, destinationPath, config);
    }
  }
};
