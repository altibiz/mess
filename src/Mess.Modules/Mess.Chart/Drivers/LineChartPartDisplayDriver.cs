using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Mess.Chart.Drivers;

public class LineChartPartDisplayDriver
  : ContentPartDisplayDriver<LineChartPart>
{
  public override IDisplayResult Display(
    LineChartPart part,
    BuildPartDisplayContext context
  )
  {
    return Combine(
      Dynamic(
          "LineChartPart_Admin",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
            model.LineChartDatasetContentTypes = _contentDefinitionManager
              .ListTypeDefinitions()
              .Where(
                contentTypeDefinition =>
                  contentTypeDefinition.GetStereotype()
                  == "ConcreteLineChartDataset"
              );
          }
        )
        .Location("Admin", "Content:10"),
      Dynamic(
          "LineChartPart_Thumbnail",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
          }
        )
        .Location("Thumbnail", "Content:10")
    );
  }

  public LineChartPartDisplayDriver(
    IStringLocalizer<LineChartPartDisplayDriver> localizer,
    IContentDefinitionManager contentDefinitionManager
  )
  {
    S = localizer;
    _contentDefinitionManager = contentDefinitionManager;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
