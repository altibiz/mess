using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Id = "Mess.Event",
  Name = "Event",
  Description = "The Event module enables the usage of an event store using MartenDb.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new[] { ManifestConstants.MessTag }
)]
