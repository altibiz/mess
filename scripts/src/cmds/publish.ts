import {
  cmd,
  dotenv,
  dotnetFmt,
  env,
  exists,
  mkdir,
  ptask,
  root,
  task,
} from "../lib/index";

export default cmd({
  usage: "publish [publish-dir]",
  description: "Publish mess",
  builder: (_) =>
    _.positional("publish-dir", {
      type: "string",
      description: "The directory to publish to",
      default: "artifacts",
    }).option("launch", {
      type: "boolean",
      description: "Launch the project after publishing",
      default: false,
    }),
})(async ({ publishDir, launch }) => {
  if (!(await exists(publishDir))) {
    await mkdir(publishDir);
  }

  env("ASPNETCORE_ENVIRONMENT", "Production");
  env("DOTNET_ENVIRONMENT", "Production");
  env("ORCHARD_APP_DATA", root("App_Data"));
  env("NODE_OPTIONS", "--no-warnings");
  env("NODE_ENV", "production");
  await dotenv("secrets.sh");

  await task("Built with yarn", "yarn build");

  await task(
    "Published with dotnet",
    `dotnet publish ${root("Mess.sln")}` +
      ` --property PublishDir=${root(publishDir)}` +
      " --property ConsoleLoggerParameters=ErrorsOnly" +
      " --property IsWebConfigTransformDisabled=true" +
      " --property DebugType=None" +
      " --property DebugSymbols=false" +
      " --configuration Release",
  );

  if (launch) {
    await ptask(
      { name: "docker", command: "docker-compose up", silent: true },
      {
        name: "dotnet",
        command: "dotnet Mess.Web.dll",
        cwd: root(publishDir),
        fmt: dotnetFmt,
      },
    );
  }
});
