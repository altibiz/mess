import {
  cmd,
  coerceValue,
  env,
  exists,
  globf,
  log,
  root,
  task,
} from "../lib/index";

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

  await task("Installed dependencies with dotnet", `dotnet restore ${root()}`);

  if (build) {
    await task("Built assets with bun", "bun assets build");
  }

  if (build || !skip.includes("test")) {
    await task("Built app with dotnet", `dotnet build ${root("Mess.sln")}`);
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
    await task("Created containers", {
      name: "docker",
      command: "docker compose up --no-start",
      unary: true,
    });

    if (!skip.includes("hooks")) {
      if (!(await exists(".husky/_/husky.sh"))) {
        await task("Set up git hooks", "bunx husky install");
      } else {
        log.info("Git hooks already set up");
      }
    }
  }
});
