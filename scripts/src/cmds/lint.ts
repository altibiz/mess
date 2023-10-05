import { cmd, root, task } from "../lib/index";

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

  await task(
    "Linted with prettier",
    "bunx prettier --check" +
      " --ignore-path .prettierignore" +
      " --cache --cache-strategy metadata" +
      " .",
  );

  if (!formatOnly) {
    // TODO: with bun
    await task("Linted workspaces", "npm run lint --workspaces");
    await task(
      "Linted projects",
      `dotnet build ${root()}` +
        ` --property "PublishDir=${root("artifacts")}"` +
        " --property:TreatWarningsAsErrors=true" +
        " --configuration Release",
    );
  }
});
