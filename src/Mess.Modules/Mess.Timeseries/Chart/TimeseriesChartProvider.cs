using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Tenants;
using Mess.Timeseries.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;

namespace Mess.Timeseries.Chart;

public record class TimeseriesChartProvider(ITenants Tenants) : IChartProvider
{
  public string Id => throw new NotImplementedException();

  public ChartSpecification? CreateChart(ChartParameters parameters)
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return null;
    }

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

  public async Task<ChartSpecification?> CreateChartAsync(
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return null;
    }

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

  public object CreateFieldEditorModel(
    BuildFieldEditorContext context,
    ContentField field,
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return new TimeseriesChartFieldEditorViewModel { };
    }

    return new TimeseriesChartFieldEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public object CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart part,
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return new TimeseriesChartPartEditorViewModel { };
    }

    return new TimeseriesChartPartEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public string GetFieldEditorShapeType(BuildFieldEditorContext context)
  {
    return "TimeseriesChartField_Edit";
  }

  public string GetPartEditorShapeType(BuildPartEditorContext context)
  {
    return "TimeseriesChartPart_Edit";
  }

  public string? ValidateParameters(ChartParameters parameters)
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return $"Field '{nameof(parameters.ProviderParameters)}' is empty";
    }

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

  public async Task<string?> ValidateParametersAsync(ChartParameters parameters)
  {
    var providerParameters =
      parameters.ProviderParameters as TimeseriesChartProviderParameters;
    if (providerParameters is null)
    {
      return $"Field '{nameof(parameters.ProviderParameters)}' is empty";
    }

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
}
