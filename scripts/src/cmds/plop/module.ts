import { cmd, plopd, task } from "../../lib/index";

const exampleName = "IotDevice";
const exampleDescription =
  "The Measurement Device module adds" +
  " support for push and pull type measurement devices.";

export default cmd({
  usage: "module <name> <description>",
  description: "Plop a new module",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      description: `Module name (like '${exampleName}')`,
    })
      .positional("description", {
        type: "string",
        description: `Module description (like '${exampleDescription}')`,
      })
      .option("format", {
        type: "boolean",
        description: "Format the code after plop",
        default: true,
      }),
})(async ({ name, description, format }) => {
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

  await plopd("module", `src/Mess.Modules/Mess.${name}`, config);
  await task(
    `Added project Mess.${name} to solution`,
    `dotnet sln add src/Mess.Modules/Mess.${name}/Mess.${name}.csproj`,
  );

  await plopd(
    "module-abstractions",
    `src/Mess.Abstractions/Mess.${name}.Abstractions`,
    config,
  );
  await task(
    `Added project Mess.${name}.Abstractions to solution`,
    `dotnet sln add src/Mess.Abstractions/Mess.${name}.Abstractions/Mess.${name}.Abstractions.csproj`,
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
