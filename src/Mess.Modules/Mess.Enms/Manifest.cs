using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.Cms.ManifestConstants;

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
    "Mess.Population",
    "Mess.Iot",
    "Mess.Chart",
    "Mess.Fields"
  }
)]
