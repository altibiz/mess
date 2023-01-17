using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;
using Mess.Tenants;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Extensions.System;
using Mess.MeasurementDevice.ViewModels;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.System.Extensions.Type;

namespace Mess.MeasurementDevice.Chart;

public class EgaugeChartProvider
  : ChartProvider<
    EgaugeChartProviderParameters,
    EgaugeChartFieldEditorViewModel,
    EgaugeChartPartEditorViewModel
  >
{
  public const string ProviderId = "Egauge";

  public override string Id => ProviderId;

  public override ChartSpecification? CreateChart(
    EgaugeChartProviderParameters providerParameters
  )
  {
    var now = DateTime.UtcNow;
    var measurements = _client.GetEgaugeMeasurements(
      providerParameters.Source,
      beginning: now.Subtract(providerParameters.HistorySpan),
      end: now
    );

    return new(
      ChartType.Line,
      new LineChartSpecification(
        providerParameters.Fields
          .Select(
            field =>
              measurements.ToLineChartDataset(
                label: field.Label,
                unit: field.Unit,
                color: field.Color,
                nameof(EgaugeMeasurementModel.Timestamp),
                field.Field
              )
          )
          .ToList()
      )
    );
  }

  public override async Task<ChartSpecification?> CreateChartAsync(
    EgaugeChartProviderParameters providerParameters
  )
  {
    var now = DateTime.UtcNow;
    var measurements = await _client.GetEgaugeMeasurementsAsync(
      providerParameters.Source,
      beginning: now.Subtract(providerParameters.HistorySpan),
      end: now
    );

    return new(
      ChartType.Line,
      new LineChartSpecification(
        providerParameters.Fields
          .Select(
            field =>
              measurements.ToLineChartDataset(
                label: field.Label,
                unit: field.Unit,
                color: field.Color,
                nameof(EgaugeMeasurementModel.Timestamp),
                field.Field
              )
          )
          .ToList()
      )
    );
  }

  public override EgaugeChartFieldEditorViewModel CreateFieldEditorModel(
    BuildFieldEditorContext context,
    ContentField field,
    EgaugeChartProviderParameters providerParameters
  )
  {
    return new EgaugeChartFieldEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public override EgaugeChartPartEditorViewModel CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart field,
    EgaugeChartProviderParameters providerParameters
  )
  {
    return new EgaugeChartPartEditorViewModel
    {
      Parameters = providerParameters
    };
  }

  public override string GetFieldEditorShapeType(
    BuildFieldEditorContext context
  )
  {
    return "EgaugeChartField_Edit";
  }

  public override string GetPartEditorShapeType(BuildPartEditorContext context)
  {
    return "EgaugeChartPart_Edit";
  }

  public override string? ValidateParameters(
    EgaugeChartProviderParameters providerParameters
  )
  {
    // TODO: validate available from database?
    if (String.IsNullOrWhiteSpace(providerParameters.Source))
    {
      return $"Field '{nameof(providerParameters.Source)}' is empty.";
    }

    if (providerParameters.HistorySpan > TimeSpan.FromDays(30))
    {
      return $"Field '{nameof(providerParameters.HistorySpan)}' is longer than 30 days";
    }

    var modelFieldAndPropertyNames =
      typeof(EgaugeMeasurementModel).GetFieldAndPropertyNames();
    foreach (var field in providerParameters.Fields)
    {
      if (String.IsNullOrWhiteSpace(field.Field))
      {
        return $"Field '{nameof(field.Field)}' is empty.";
      }

      if (!modelFieldAndPropertyNames.Any(name => name == field.Field))
      {
        return $"Field '{nameof(field.Field)}' must be one of {string.Join(", ", modelFieldAndPropertyNames)}";
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

  public EgaugeChartProvider(ITenants tenants, IMeasurementClient client)
  {
    _tenants = tenants;
    _client = client;
  }

  private readonly ITenants _tenants;
  private readonly IMeasurementClient _client;
}
