import {
  Argv,
  cmd,
  dotnetFmt,
  env,
  exists,
  log,
  ptask,
  rmrf,
  root,
  task,
  yarnFmt,
} from "../lib/index";

export default cmd({
  usage: "dev",
  description: "Start the development environment",
  builder: (_) =>
    _.option("update", {
      type: "string",
      array: true,
      description: "Run and pass update argument to yarn publishing",
      default: [],
    })
      .option("push", {
        type: "string",
        array: true,
        description: "Run and pass push argument to yarn publishing",
        default: [],
      })
      .option("clean", {
        type: "boolean",
        description: "Clean relevant artifacts before watch",
        default: false,
      })
      .option("debug", {
        type: "boolean",
        description:
          "Don't run dotnet command because it is open with a debugger",
        default: false,
      }) as Argv<{
      update: string[];
      push: string[];
      debug: boolean;
      clean: boolean;
    }>,
})(async ({ push, update, debug, clean }) => {
  env("ASPNETCORE_ENVIRONMENT", "Development");
  env("DOTNET_ENVIRONMENT", "true");
  env("ORCHARD_APP_DATA", root("App_Data"));
  env("NODE_OPTIONS", "--no-warnings");
  env("NODE_ENV", "development");

  if (clean) {
    if (await exists("App_Data")) {
      await rmrf("App_Data");
      log.info("Cleaned App_Data");
    }

    await task("Cleaned docker containers and volumes", {
      name: "docker",
      command: "docker-compose down -v",
      unary: true,
    });
  }

  await task(
    "Built assets with yarn so that dotnet watch is aware of artifacts",
    "yarn assets build",
  );

  const debugCommands = debug
    ? []
    : [
        {
          name: "dotnet",
          command:
            "dotnet watch run" +
            ` --project ${root("src/Mess.Web/Mess.Web.csproj")}` +
            " --configuration Debug" +
            " --property:consoleLoggerParameters=ErrorsOnly",

          fmt: dotnetFmt,
        },
      ];

  const pushArgs = push.map((push) => `--push ${push}`).join(" ");
  const updateArgs = update.map((update) => `--update ${update}`).join(" ");
  const publishCommands =
    pushArgs.length || updateArgs.length
      ? [
          {
            name: "publish",
            command: "yarn publishing dev " + `${pushArgs} ${updateArgs}`,
          },
        ]
      : [];

  await ptask(
    { name: "docker", command: "docker-compose up", silent: true },
    {
      name: "yarn",
      command: "yarn assets watch",
      fmt: yarnFmt,
    },
    ...debugCommands,
    ...publishCommands,
  );
});
