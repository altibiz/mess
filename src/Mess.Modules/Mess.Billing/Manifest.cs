using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Billing",
  Name = "Billing",
  Description = "The Billing module adds content definitions and a background job to create bills every month.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new string[] { }
)]
