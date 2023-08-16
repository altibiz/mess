import { cmd, env, task } from "../lib/index";

export default cmd({
  usage: "format",
  description: "Format the repository",
})(async () => {
  env("NODE_OPTIONS", "--no-warnings");

  await task("Formatted with csharpier", "dotnet csharpier .");

  await task(
    "Formatted with prettier",
    "yarn prettier --write --ignore-path .prettierignore .",
  );
});
