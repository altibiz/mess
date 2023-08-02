import { env, ptask, cmd } from "../lib/index";

export default cmd({
  usage: "test",
  description: "Run tests (arguments after '--' are passed to dotnet test)",
})(async ({ _ }) => {
  env("ASPNETCORE_ENVIRONMENT", "Development");
  env("DOTNET_ENVIRONMENT", "http://localhost:5000");

  ptask(
    {
      name: "docker",
      command: "docker compose up",
    },
    {
      name: "dotnet",
      command: `dotnet test ${_.join(" ")}`,
    },
  );
});
