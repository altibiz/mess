using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Iot",
  Name = "IOT",
  Description = "The IOT module adds support for communication with IOT devices.",
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
