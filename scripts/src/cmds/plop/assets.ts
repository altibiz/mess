import { cmd, plopd, task } from "../../lib/index";

const exampleName = "IotDevice";

export default cmd({
  usage: "module <name> <description>",
  description: "Plop assets for a module or theme",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      description: `Module or theme name (like '${exampleName}')`,
    }).option("format", {
      type: "boolean",
      description: "Format the code after plop",
      default: true,
    }),
})(async ({ name, format }) => {
  const lowercaseName = name.toLowerCase();
  const longName = name.replace(/(a-z)(A-Z)/g, "$1 $2");
  const hyphenatedName = name.replace(/([a-z])([A-Z])/g, "$1-$2").toLowerCase();

  const config = {
    name,
    lowercaseName,
    longName,
    hyphenatedName,
  };

  await plopd("assets", `src/Mess.Assets/Mess.${name}`, config);

  await task(
    `Added package src/Mess.Assets/Mess.${name} to workspace`,
    "yarn install",
  );

  if (format) {
    await task("Formatted with csharpier", "dotnet csharpier .");

    await task(
      "Formatted with prettier",
      "yarn prettier --write" +
        " --ignore-path .prettierignore" +
        " --cache --cache-strategy metadata" +
        " .",
    );
  }
});
