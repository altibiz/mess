using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Timeseries",
  Name = "Timeseries",
  Description = "The Timeseries module enables the usage of a timeseries database using TimescaleDb.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new[] { ManifestConstants.MessTag },
  Dependencies = new[] { "Mess.Relational" }
)]
