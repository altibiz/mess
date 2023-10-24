import { cmd, root, task } from "../lib/index";

export default cmd({
  usage: "lint",
  description: "Lint the repository",
  builder: (_) =>
    _.option("format-only", {
      type: "boolean",
      description: "Only check formatting",
      default: false,
    }).option("skip-format", {
      type: "boolean",
      description: "Skip checking formatting",
      default: false,
    }),
})(async ({ formatOnly, skipFormat }) => {
  if (!skipFormat) {
    await task("Linted with csharpier", "dotnet csharpier . --check");

    await task(
      "Linted with prettier",
      "yarn prettier --check" +
        " --ignore-path .prettierignore" +
        " --cache --cache-strategy metadata" +
        " .",
    );
  }

  if (!formatOnly) {
    await task("Linted workspaces", "yarn workspaces foreach -ip run lint");
    await task(
      "Linted projects",
      `dotnet build ${root()}` +
        ` --property "PublishDir=${root("artifacts")}"` +
        " --property:TreatWarningsAsErrors=true" +
        " --configuration Release",
    );
  }
});
