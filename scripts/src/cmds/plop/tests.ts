import { cmd, plopd, task } from "../../lib/index";

const exampleName = "IotDevice";

export default cmd({
  usage: "module <name> <description>",
  description: "Plop tests for a module",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      description: `Module name (like '${exampleName}')`,
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
