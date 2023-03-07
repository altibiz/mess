using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "Event Store",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
)]

[assembly: Feature(
  Id = "Mess.EventStore",
  Name = "Event Store",
  Description = "The Event Store module enables the usage of an event store using MartenDb.",
  Category = ManifestConstants.Category
)]
