using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Iot",
  Name = "Measurement Device",
  Description = "The Measurement Device module adds support for push and pull type measurement devices.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new[] { ManifestConstants.MessTag },
  Dependencies = new[]
  {
    "OrchardCore.Contents",
    "OrchardCore.ContentFields",
    "OrchardCore.Title",
    "Mess.Timeseries",
    "Mess.Fields"
  }
)]
