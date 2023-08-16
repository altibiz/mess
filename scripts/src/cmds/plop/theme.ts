import { cmd, plopd } from "../../lib/index";

export default cmd({
  usage: "theme <name> <description>",
  description: "Plop a new theme",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      group: "Theme name",
    }).positional("description", {
      type: "string",
      group: "Theme description",
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
});
