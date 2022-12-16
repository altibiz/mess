using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "Mess.Timeseries",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.Timeseries",
  Category = "Content Management",
  Dependencies = new[] { "OrchardCore.Tenants", "Mess.EventStore" }
)]
