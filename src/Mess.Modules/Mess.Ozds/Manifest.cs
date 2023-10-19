using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Ozds",
  Name = "Ozds",
  Description = "The Ozds project module.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new string[]
  {
    "OrchardCore.Navigation",
    "OrchardCore.Roles",
    "OrchardCore.Users",
    "OrchardCore.Contents",
    "OrchardCore.ContentFields",
    "OrchardCore.ContentFields.Indexing.SQL",
    "OrchardCore.ContentFields.Indexing.SQL.UserPicker",
    "OrchardCore.Title",
    "Mess.Timeseries",
    "Mess.Iot",
    "Mess.Billing",
    "Mess.Chart",
    "Mess.Fields"
  }
)]
