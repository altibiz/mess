import { cmd, task, exists, globf, env, coerceValue, log } from "../lib/index";

export default cmd({
  usage: "prepare",
  description: "Prepare the repository for development",
  builder: (_) =>
    _.option("skip", {
      type: "string",
      array: true,
      description: "Skip a step",
      default: [],
      choices: ["test", "hooks"],
      coerce: coerceValue<"test" | "hooks">(),
    }).option("build", {
      type: "boolean",
      description: "Build the project",
      default: false,
    }),
})(async ({ skip, build }) => {
  env("NODE_OPTIONS", "--no-warnings");

  await task("Installed tools with dotnet", "dotnet tool restore");

  await task("Installed dependencies with dotnet", "dotnet restore");

  if (build) {
    await task("Built with yarn", "yarn build");
    await task("Built with dotnet", "dotnet build");
  }

  if (!skip.includes("test")) {
    await task(
      "Installed playwright",
      `dotnet pwsh ${await globf(
        "test/Mess.System.Test/bin/Debug/**/playwright.ps1",
        "Playwright install script not found",
      )} install --with-deps`,
    );
  }

  if (!env("CI")) {
    await task("Created containers", "docker compose up --no-start");

    // TODO: currently we have no secrets - technically we could fetch them
    // from cloud storage here

    // if (!(await exists("secrets.json")) || !(await exists("secrets.sh"))) {
    //   throw new Error(
    //     "File 'secrets.json' or 'secrets.sh' not found." +
    //       "Please copy over secrets to the worktree.",
    //   );
    // }

    // await task(
    //   "Set up secrets",
    //   `dotnet user-secrets set --project ${root(
    //     "src/Mess.Web/Mess.Web.csproj",
    //   )} <${root("secrets.json")}`,
    // );

    // if (!(await exists("secrets"))) {
    //   await mkdir("secrets");
    // }
    // await cp("secrets.json", "secrets/secrets.json");
    // await cp("secrets.sh", "secrets/secrets.sh");

    if (!skip.includes("hooks")) {
      if (!(await exists(".husky/_/husky.sh"))) {
        await task("Set up git hooks", "yarn husky install");
      } else {
        log.info("Git hooks already set up");
      }
    }
  }
});
