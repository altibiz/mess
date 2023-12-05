using OrchardCore.Modules.Manifest;
using ManifestConstants = Mess.Cms.ManifestConstants;

[assembly: Module(
  Id = "Mess.Chart",
  Name = "Chart",
  Description =
    "The Chart module enables the creation and display of charts using Chart.js.",
  Author = ManifestConstants.Author,
  Website = ManifestConstants.Website,
  Version = ManifestConstants.Version,
  Category = ManifestConstants.Category,
  Tags = new[] { ManifestConstants.MessTag },
  Dependencies = new[]
  {
    "OrchardCore.Resources",
    "OrchardCore.Contents",
    "OrchardCore.ContentFields",
    "OrchardCore.Flows",
    "OrchardCore.Title",
    "Etch.OrchardCore.Fields.Colour",
    "Mess.Fields",
    "Mess.Blazor"
  }
)]
