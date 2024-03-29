using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.Cms.ManifestConstants;

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
    "OrchardCore.Contents",
    "OrchardCore.ContentFields",
    "OrchardCore.ContentFields.Indexing.SQL",
    "OrchardCore.ContentFields.Indexing.SQL.UserPicker",
    "Mess.Population",
    "Mess.Timeseries",
    "Mess.Iot",
    "Mess.Chart",
    "Mess.Fields"
  }
)]
