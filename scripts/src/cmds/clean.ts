import { cmd, exists, log, rmrf, task } from "../lib/index";

const exceptions = [
  "!.husky/",
  "!.husky/_/",
  "!.husky/_/*",
  "!.yarn/**",
  "!.pnp.cjs",
  "!.pnp.loader.mjs",
  "!.vs/",
  "!.vs/**",
  "!**/*.csproj.user",
  "!.vscode/**",
  "!.env",
  "!secrets.json",
  "!secrets.sh",
  "!secrets/",
  "!secrets/**",
  "!App_Data/",
  "!.direnv",
]
  .map((exception) => `-e '${exception}'`)
  .join(" ");

export default cmd({
  usage: "clean",
  description: "Clean the repository of artifacts",
  builder: (_) =>
    _.option("all", {
      type: "boolean",
      description: "Clean the repository of binary artifacts",
      default: false,
      alias: "a",
    }),
})(async ({ all }) => {
  if (all) {
    await task("Cleaned artifacts", `git clean -Xdf ${exceptions}`);
  }

  if (await exists("App_Data")) {
    await rmrf("App_Data");
    log.info("Cleaned App_Data");
  }

  await task("Cleaned docker containers and volumes", {
    name: "docker",
    command: "docker compose down -v",
  });
});
