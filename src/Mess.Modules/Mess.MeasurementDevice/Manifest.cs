using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "Measurement Device",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
)]

[assembly: Feature(
  Id = "Mess.MeasurementDevice",
  Name = "Measurement Device",
  Description = "The Measurement Device module adds support for push and pull type measurement devices.",
  Category = ManifestConstants.Category,
  Dependencies = new[]
  {
    "OrchardCore.ContentTypes",
    "OrchardCore.ContentFields",
    "OrchardCore.Title",
    "Mess.Timeseries",
  }
)]
