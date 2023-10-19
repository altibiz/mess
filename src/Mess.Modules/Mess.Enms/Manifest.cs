using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Enms",
  Name = "Enms",
  Description = "The Enms project module.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new string[]
  {
    "OrchardCore.Contents",
    "OrchardCore.ContentFields",
    "OrchardCore.Title",
    "Mess.Iot",
    "Mess.Chart",
    "Mess.Population",
    "Mess.Fields",
  }
)]
