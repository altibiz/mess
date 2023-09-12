import { cmd, plopd, task } from "../../lib/index";

const exampleName = "Forttech";
const exampleDescription = "The Forttech company theme.";

export default cmd({
  usage: "theme <name> <description>",
  description: "Plop a new theme",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      description: `Theme name (like '${exampleName}')`,
    })
      .positional("description", {
        type: "string",
        description: `Theme description (like '${exampleDescription}')`,
      })
      .option("assets", {
        type: "boolean",
        description: "Also plop an assets package",
        default: true,
      })
      .option("format", {
        type: "boolean",
        description: "Format the code after plop",
        default: true,
      }),
})(async ({ name, description, assets, format }) => {
  const lowercaseName = name.toLowerCase();
  const longName = name.replace(/(a-z)(A-Z)/g, "$1 $2");
  const hyphenatedName = name.replace(/([a-z])([A-Z])/g, "$1-$2").toLowerCase();

  const config = {
    name,
    lowercaseName,
    longName,
    hyphenatedName,
    description,
  };

  await plopd("theme", `src/Mess.Themes/Mess.${name}`, config);
  await task(
    `Added project Mess.${name} to solution`,
    `dotnet sln add src/Mess.Themes/Mess.${name}/Mess.${name}.csproj`,
  );

  if (assets) {
    await plopd("assets", `src/Mess.Themes/Mess.${name}/Assets`, config);

    await task(
      `Added package src/Mess.Themes/Mess.${name}/Assets to workspace`,
      "yarn install",
    );
  }

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
