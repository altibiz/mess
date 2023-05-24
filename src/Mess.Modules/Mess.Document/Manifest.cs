using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "Document",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new string[] { ManifestConstants.MessTag }
)]

[assembly: Feature(
  Id = "Mess.Document",
  Name = "Document",
  Description = "The Document module enables the upload, download and display of documents in the database.",
  Category = ManifestConstants.Category,
  Dependencies = new string[] { }
)]
