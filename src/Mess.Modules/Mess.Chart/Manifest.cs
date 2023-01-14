using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "Mess.Chart",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.Chart",
  Category = "Content Management",
  Dependencies = new[]
  {
    "OrchardCore.ContentTypes",
    "OrchardCore.ContentFields"
  }
)]
