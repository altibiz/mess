import { task, root, cmd } from "../lib/index";

export default cmd({
  usage: "lint",
  description: "Lint the repository",
  builder: (_) =>
    _.option("format-only", {
      type: "boolean",
      description: "Only check formatting",
      default: false,
    }),
})(async ({ formatOnly }) => {
  await task("Linted with csharpier", "dotnet csharpier . --check");

  await task("Linted with prettier", "yarn lint:prettier");

  if (!formatOnly) {
    await task("Linted workspaces", "yarn lint:workspaces");
    await task(
      "Linted projects",
      "dotnet build" +
        ` --property "PublishDir=${root("artifacts")}"` +
        " --property:TreatWarningsAsErrors=true" +
        " --configuration Release",
    );
  }
});
