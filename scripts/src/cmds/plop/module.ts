import { cmd, plopd } from "../../lib/index";

export default cmd({
  usage: "module <name> <description>",
  description: "Plop a new module",
  builder: (_) =>
    _.positional("name", {
      type: "string",
      group: "Module name",
    }).positional("description", {
      type: "string",
      group: "Module description",
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

  await plopd("module", `src/Mess.Modules/Mess.${name}`, config);
  await plopd(
    "module-abstractions",
    `src/Mess.Abstractions/Mess.${name}.Abstractions`,
    config,
  );
  await plopd("module-test", `test/Mess.Modules/Mess.${name}.Test`, config);
  await plopd(
    "module-test-abstractions",
    `test/Mess.Abstractions/Mess.${name}.Test.Abstractions`,
    config,
  );
});
