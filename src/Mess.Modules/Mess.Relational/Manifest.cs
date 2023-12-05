using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.Cms.ManifestConstants;

[assembly: Module(
  Id = "Mess.Relational",
  Name = "Relational",
  Description =
    "The Relational module adds base functionalities to integrate a classic relational database into other modules using Entity Framework Core.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new string[] { }
)]
