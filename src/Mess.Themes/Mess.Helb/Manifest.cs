using Mess.Cms;
using OrchardCore.DisplayManagement.Manifest;

[assembly: Theme(
  Id = "Mess.Helb",
  Name = "The Helb Theme",
  Description = "The HELB company theme.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new string[] { ManifestConstants.MessTag },
  Dependencies = new[] { "Mess.Blazor" }
)]
