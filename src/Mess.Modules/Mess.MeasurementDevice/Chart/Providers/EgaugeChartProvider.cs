using Mess.Tenants;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Extensions.System;
using Mess.MeasurementDevice.Chart.ViewModels;
using Mess.MeasurementDevice.Chart.Models;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.System.Extensions.Type;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Chart.Providers;

public class EgaugeChartProvider
  : ChartDataProvider<EgaugeChartParameters, EgaugeChartPartEditViewModel>
{
  public const string ProviderId = "Egauge";

  public override string Id => ProviderId;

  public override ChartSpecification? CreateChart(
    EgaugeChartParameters providerParameters
  )
  {
    var now = DateTime.UtcNow;
    var measurements = _client.GetEgaugeMeasurements(
      providerParameters.Source,
      beginning: now.Subtract(providerParameters.History.ToTimeSpan()),
      end: now
    );

    return CreateChart(providerParameters, measurements);
  }

  public override async Task<ChartSpecification?> CreateChartAsync(
    EgaugeChartParameters providerParameters
  )
  {
    var now = DateTime.UtcNow;
    var measurements = await _client.GetEgaugeMeasurementsAsync(
      providerParameters.Source,
      beginning: now.Subtract(providerParameters.History.ToTimeSpan()),
      end: now
    );

    return CreateChart(providerParameters, measurements);
  }

  private ChartSpecification? CreateChart(
    EgaugeChartParameters providerParameters,
    IEnumerable<EgaugeMeasurementModel> measurements
  ) =>
    new(
      ChartType.Line,
      new LineChartSpecification(
        providerParameters.Fields
          .Select(field =>
          {
            var fieldPart = field.As<EgaugeChartFieldPart>();
            return measurements.ToLineChartDataset(
              label: fieldPart.Label,
              unit: fieldPart.Unit,
              color: fieldPart.Color,
              nameof(EgaugeMeasurementModel.Timestamp),
              fieldPart.Field
            );
          })
          .ToList()
      )
    );

  public override string? ValidateParameters(
    EgaugeChartParameters providerParameters
  )
  {
    // TODO: validate available from database?
    if (String.IsNullOrWhiteSpace(providerParameters.Source))
    {
      return $"Field '{nameof(providerParameters.Source)}' is empty.";
    }

    var modelFieldAndPropertyNames =
      typeof(EgaugeMeasurementModel).GetFieldAndPropertyNames();
    foreach (var field in providerParameters.Fields)
    {
      var fieldPart = field.As<EgaugeChartFieldPart>();
      if (String.IsNullOrWhiteSpace(fieldPart.Field))
      {
        return $"Field '{nameof(fieldPart.Field)}' is empty.";
      }

      if (!modelFieldAndPropertyNames.Any(name => name == fieldPart.Field))
      {
        return $"Field '{nameof(fieldPart.Field)}' must be one of {string.Join(", ", modelFieldAndPropertyNames)}";
      }

      if (String.IsNullOrWhiteSpace(fieldPart.Label))
      {
        return $"Field '{nameof(fieldPart.Label)}' is empty.";
      }

      if (String.IsNullOrWhiteSpace(fieldPart.Color))
      {
        return $"Field '{nameof(fieldPart.Color)}' is empty.";
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
