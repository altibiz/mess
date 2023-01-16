using OrchardCore.Modules.Manifest;

[assembly: Module(
  Name = "Mess.MeasurementDevice",
  Author = "Altibiz",
  Website = "https://altibiz.com",
  Version = "0.0.1",
  Description = "Mess.MeasurementDevice",
  Category = "Content Management",
  Dependencies = new[] { "Mess.EventStore, Mess.Timeseries", "Mess.Chart" }
)]
