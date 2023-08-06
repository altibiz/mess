import {
  env,
  ptask,
  root,
  cmd,
  dotnetFmt,
  yarnFmt,
  task,
  Argv,
} from "../lib/index";

export default cmd({
  usage: "watch",
  description: "Watch for changes and restart the server",
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
      .option("debug", {
        type: "boolean",
        description:
          "Don't run dotnet command because it is open with a debugger",
        default: false,
      }) as Argv<{
      update: string[];
      push: string[];
      debug: boolean;
    }>,
})(async ({ push, update, debug }) => {
  env("ASPNETCORE_ENVIRONMENT", "Development");
  env("DOTNET_ENVIRONMENT", "true");
  env("ORCHARD_APP_DATA", root("App_Data"));
  env("NODE_OPTIONS", "--no-warnings");
  env("NODE_ENV", "development");

  await task(
    "Built with yarn so that dotnet watch is aware of artifacts",
    "yarn build",
  );

  const debugCommands = debug
    ? []
    : [
        {
          name: "dotnet",
          command:
            "dotnet watch run" +
            " --configuration Debug" +
            " --property:consoleLoggerParameters=ErrorsOnly" +
            ` --project ${root("src/Mess.Web/Mess.Web.csproj")}`,

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
            command:
              "yarn workspace @mess/publishing dev " +
              `${pushArgs} ${updateArgs}`,
          },
        ]
      : [];

  await ptask(
    { name: "docker", command: "docker-compose up", silent: true },
    {
      name: "yarn",
      command: "yarn watch",
      fmt: yarnFmt,
    },
    ...debugCommands,
    ...publishCommands,
  );
});
