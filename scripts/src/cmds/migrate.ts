import {
  cmd,
  coerceRegex,
  exists,
  globf,
  log,
  mkdir,
  mv,
  rmrf,
  root,
  task,
} from "../lib/index";

export default cmd({
  usage: "migrate <project> <name>",
  description: "Create an EF migration",
  builder: (_) =>
    _.positional("project", {
      type: "string",
      description: "The project to create the migration for",
      demandOption: true,
      coerce: coerceRegex(
        /^[A-Za-z.]+$/,
        "Project name must be a valid C# namespace",
      ),
    })
      .positional("name", {
        type: "string",
        description: "The name of the migration",
        demandOption: true,
        coerce: coerceRegex(
          /^[A-Za-z]+$/,
          "Migration name must be a valid C# identifier",
        ),
      })
      .option("format", {
        type: "boolean",
        description: "Format the migration with csharpier",
        default: true,
      }),
})(async ({ project, name, format }) => {
  let isInitial = false;
  if (!(await exists(`src/Mess.Modules/${project}/Timeseries/Migrations`))) {
    isInitial = true;
    await mkdir(`src/Mess.Modules/${project}/Timeseries/Migrations`);
  }

  await task(
    "Created migration",
    "dotnet ef" +
      ` --startup-project ${root("src/Mess.Web/Mess.Web.csproj")}` +
      ` --project ${root(`src/Mess.Modules/${project}/${project}.csproj`)}` +
      " migrations add" +
      " --output-dir Timeseries/Migrations" +
      ` --namespace ${project}.Timeseries.Migrations` +
      ` ${name}`,
  );

  if (isInitial) {
    await mv(
      await globf(
        `src/Mess.Modules/${project}/Mess/**/Timeseries/*Snapshot.cs`,
        "Initial migration snapshot not found",
      ),
      `src/Mess.Modules/${project}/Timeseries/Migrations`,
    );
    await rmrf(`src/Mess.Modules/${project}/Mess`);
    log.info("Moved initial migration snapshot");
  }

  if (format) {
    await task(
      "Formatted with csharpier",
      `dotnet csharpier ${root(
        `src/Mess.Modules/${project}/Timeseries/Migrations`,
      )}`,
    );
  }
});
