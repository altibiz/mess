using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "Timeseries",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
)]

[assembly: Feature(
  Id = "Mess.Timeseries",
  Name = "Timeseries",
  Description = "The Timeseries module enables the usage of a timeseries database using TimescaleDb.",
  Category = ManifestConstants.Category
)]
