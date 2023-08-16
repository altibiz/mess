import { cmd, exists, log, rmrf, task } from "../lib/index";

const exceptions = [
  "-e '!.husky/'",
  "-e '!.husky/_/'",
  "-e '!.husky/_/*'",
  "-e '!.yarn/**'",
  "-e '!.pnp.cjs'",
  "-e '!.pnp.loader.mjs'",
  "-e '!.vs/'",
  "-e '!.vs/**'",
  "-e '!**/*.csproj.user'",
  "-e '!.vscode/**'",
  "-e '!.env'",
  "-e '!secrets.json'",
  "-e '!secrets.sh'",
  "-e '!secrets/'",
  "-e '!secrets/**'",
  "-e '!App_Data/'",
].join(" ");

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
    command: "docker-compose down -v",
    unary: true,
  });
});
