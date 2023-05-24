import * as globPath from "path/posix";
import * as path from "path";
import * as fs from "fs/promises";
import * as glob from "glob";
import { Create } from "./templates";
import args from "./args";

(async () => {
  const rootPath = globPath.resolve(globPath.join(__dirname, ".."));
  const assetsPath = globPath.join(rootPath, "assets");
  const templatesPath = globPath.join(assetsPath, "templates");
  const templatePaths = await glob.glob(`${templatesPath}/*.*.hbs`);
  const templates = Object.fromEntries(
    await Promise.all(
      templatePaths.map(async (templatePath) => {
        const buffer = await fs.readFile(templatePath);
        const dispatcherId = path.parse(path.parse(templatePath).name).name;
        const createImport = await import(`./templates/${dispatcherId}`);
        return [
          dispatcherId,
          {
            template: buffer.toString(),
            create: createImport.default,
          },
        ];
      }),
    ),
  ) as Record<string, { template: string; create: Create }>;

  // eslint-disable-next-line no-constant-condition
  while (true) {
    for (const [dispatcherId, { template, create }] of Object.entries(
      templates,
    )) {
      const message = await create(template);
      const messages = Array.isArray(message) ? message : [message];
      const uri = `${args.baseUri}/${dispatcherId}`;
      for (const subMessage of messages) {
        try {
          await fetch(uri, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: subMessage,
          });
        } catch (e) {
          console.error(e);
        }
      }

      console.log(`Sent ${messages.length} messages to ${uri}`);
    }

    await new Promise((resolve) => setTimeout(resolve, 5000));
  }
})();
