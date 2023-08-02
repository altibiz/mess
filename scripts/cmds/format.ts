import { task, cmd, env } from "../lib/index";

export default cmd({
  usage: "format",
  description: "Format the repository",
})(async () => {
  env("NODE_OPTIONS", "--no-warnings");

  await task("Formatted with prettier", "yarn format");

  await task("Formatted with csharpier", "dotnet csharpier .");
});
