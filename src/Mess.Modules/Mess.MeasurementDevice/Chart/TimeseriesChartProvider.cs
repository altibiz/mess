using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Tenants;
using Mess.Timeseries.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;

namespace Mess.Timeseries.Chart;

public class TimeseriesChartProvider
  : ChartProvider<
    TimeseriesChartProviderParameters,
    TimeseriesChartFieldEditorViewModel,
    TimeseriesChartPartEditorViewModel
  >
{
  public const string ProviderId = "Timeseries";

  public override string Id => ProviderId;

  public override ChartSpecification? CreateChart(
    TimeseriesChartProviderParameters providerParameters
  )
  {
    return new(
      ChartType.Line,
      new LineChartSpecification(
        providerParameters.Fields
          .Select(
            field =>
              new LineChartDataset(
                Label: field.Label,
                Unit: field.Unit,
                Color: field.Color,
                // TODO: data fetching
                Data: new List<LineChartData>()
              )
          )
          .ToList()
      )
    );
  }

  public override TimeseriesChartFieldEditorViewModel CreateFieldEditorModel(
    BuildFieldEditorContext context,
    ContentField field,
    TimeseriesChartProviderParameters providerParameters
  )
  {
    return new TimeseriesChartFieldEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public override TimeseriesChartPartEditorViewModel CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart field,
    TimeseriesChartProviderParameters providerParameters
  )
  {
    return new TimeseriesChartPartEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public override string GetFieldEditorShapeType(
    BuildFieldEditorContext context
  )
  {
    return "TimeseriesChartField_Edit";
  }

  public override string GetPartEditorShapeType(BuildPartEditorContext context)
  {
    return "TimeseriesChartPart_Edit";
  }

  public override string? ValidateParameters(
    TimeseriesChartProviderParameters providerParameters
  )
  {
    if (providerParameters.HistorySpan > TimeSpan.FromDays(30))
    {
      return $"Field '{nameof(providerParameters.HistorySpan)}' is longer than 30 days";
    }

    foreach (var field in providerParameters.Fields)
    {
      if (String.IsNullOrWhiteSpace(field.Field))
      {
        return $"Field '{nameof(field.Field)}' is empty.";
      }

      if (String.IsNullOrWhiteSpace(field.Label))
      {
        return $"Field '{nameof(field.Label)}' is empty.";
      }

      if (String.IsNullOrWhiteSpace(field.Color))
      {
        return $"Field '{nameof(field.Color)}' is empty.";
      }
    }

    return null;
  }

  public TimeseriesChartProvider(ITenants tenants)
  {
    _tenants = tenants;
  }

  private readonly ITenants _tenants;
}
