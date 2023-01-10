using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "Mess.EventStore",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.EventStore",
  Category = "Content Management",
  Dependencies = new[] { "OrchardCore.Tenants", "Mess.Timeseries" }
)]
