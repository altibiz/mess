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
    }).positional("description", {
      type: "string",
      description: `Theme description (like '${exampleDescription}')`,
    }),
})(async ({ name, description }) => {
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

  await plopd("theme", `src/Mess.Themes/Mess.${name}`, config);
  await task(
    `Added project Mess.${name} to solution`,
    `dotnet sln add src/Mess.Themes/Mess.${name}/Mess.${name}.csproj`,
  );
});
