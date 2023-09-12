import { cmd, plopd, task } from "../../lib/index";

const exampleName = "MeasurementDevice";
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
      })
      .option("test", {
        type: "boolean",
        description: "Plop test projects",
        default: true,
      })
      .option("assets", {
        type: "boolean",
        description: "Also plop an assets package",
        default: false,
      }),
})(async ({ name, description, test, assets, format }) => {
  const lowercaseName = name.toLowerCase();
  const longName = name.replace(/(a-z)(A-Z)/g, "$1 $2");
  const hyphenatedName = name.replace(/([a-z])([A-Z])/g, "$1-$2");

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

  if (test) {
    await plopd("module-test", `test/Mess.Modules/Mess.${name}.Test`, config);
    await task(
      `Added project Mess.${name}.Test to solution`,
      `dotnet sln add test/Mess.Modules/Mess.${name}.Test/Mess.${name}.Test.csproj`,
    );

    await plopd(
      "module-test-abstractions",
      `test/Mess.Abstractions/Mess.${name}.Test.Abstractions`,
      config,
    );
    await task(
      `Added project Mess.${name}.Test.Abstractions to solution`,
      `dotnet sln add test/Mess.Abstractions/Mess.${name}.Test.Abstractions/Mess.${name}.Test.Abstractions.csproj`,
    );
  }

  if (assets) {
    await plopd("assets", `src/Mess.Modules/Mess.${name}/Assets`, config);

    await task(
      `Added package src/Mess.Modules/Mess.${name}/Assets to workspace`,
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
