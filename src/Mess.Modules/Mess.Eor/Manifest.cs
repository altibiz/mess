using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Eor",
  Name = "Eor",
  Description = "The Eor project module.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new string[]
  {
    "OrchardCore.Navigation",
    "OrchardCore.Users",
    "OrchardCore.Roles",
    "OrchardCore.ContentFields",
    "Mess.Timeseries",
    "Mess.Iot",
    "Mess.Chart",
    "Mess.Fields",
  }
)]
