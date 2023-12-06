import { cmd, env, root, task } from "../lib/index";

export default cmd({
  usage: "format",
  description: "Format the repository",
})(async () => {
  env("NODE_OPTIONS", "--no-warnings");

  await task(
    "Formatted with resharper",
    `dotnet jb cleanupcode ${root("Mess.sln")}` +
      " --no-build" +
      " --verbosity=ERROR" +
      ` --caches-home=${root(".jb/cache")}` +
      ` -o=${root(".jb/jb.log")}}`,
  );

  await task(
    "Formatted with prettier",
    "yarn prettier --write" +
      " --ignore-path .gitignore" +
      " --ignore-path .prettierignore" +
      " --cache --cache-strategy metadata" +
      " .",
  );
});
