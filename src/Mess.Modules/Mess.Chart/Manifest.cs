using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.OrchardCore.ManifestConstants;

[assembly: Module(
  Name = "Chart",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Tags = new[] { ManifestConstants.MessTag, ManifestConstants.OzdsTag }
)]

[assembly: Feature(
  Id = "Mess.Chart",
  Name = "Chart",
  Description = "The Chart module enables the creation and display of charts using Chart.js.",
  Category = ManifestConstants.Category,
  Dependencies = new[]
  {
    "OrchardCore.ContentTypes",
    "OrchardCore.ContentFields",
    "OrchardCore.Flows",
    "OrchardCore.Title",
    "Etch.OrchardCore.Fields.Colour",
    "Etch.OrchardCore.Fields.MultiSelect"
  }
)]
