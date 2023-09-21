using Mess.MeasurementDevice.Abstractions.Client;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.ContentFields.Abstractions;
using Mess.OrchardCore;

namespace Mess.MeasurementDevice.Chart.Providers;

public class AbbChartProvider : ChartProvider
{
  public const string ChartContentType = "AbbMeasurementDevice";

  public override string ContentType => ChartContentType;

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(AbbMeasurement.CurrentL1),
      nameof(AbbMeasurement.CurrentL2),
      nameof(AbbMeasurement.CurrentL3),
      nameof(AbbMeasurement.VoltageL1),
      nameof(AbbMeasurement.VoltageL2),
      nameof(AbbMeasurement.VoltageL3),
      nameof(AbbMeasurement.ActivePowerL1),
      nameof(AbbMeasurement.ActivePowerL2),
      nameof(AbbMeasurement.ActivePowerL3),
      nameof(AbbMeasurement.ReactivePowerL1),
      nameof(AbbMeasurement.ReactivePowerL2),
      nameof(AbbMeasurement.ReactivePowerL3),
      nameof(AbbMeasurement.ApparentPowerL1),
      nameof(AbbMeasurement.ApparentPowerL2),
      nameof(AbbMeasurement.ApparentPowerL3),
      nameof(AbbMeasurement.PowerFactorL1),
      nameof(AbbMeasurement.PowerFactorL2),
      nameof(AbbMeasurement.PowerFactorL3),
    };

  public override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    ContentItem metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var abbMeasurementDevice = metadata.AsContent<AbbMeasurementDeviceItem>();
    if (abbMeasurementDevice == null)
    {
      return default;
    }

    var now = DateTime.UtcNow;
    var measurements = await _client.GetAbbMeasurementsAsync(
      abbMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text,
      now.Subtract(chart.TimeseriesChartPart.Value.History.Value.ToTimeSpan()),
      now
    );

    return new TimeseriesChartDescriptor(
      RefreshInterval: chart.TimeseriesChartPart.Value.RefreshInterval.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      History: chart.TimeseriesChartPart.Value.History.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      Datasets: datasets
        .Select(
          dataset =>
            new TimeseriesChartDatasetDescriptor(
              Label: dataset.TimeseriesChartDatasetPart.Value.Label.Text,
              Color: dataset.TimeseriesChartDatasetPart.Value.Color.Value,
              Datapoints: measurements
                .Select(
                  measurement =>
                    new TimeseriesChartDatapointDescriptor(
                      X: measurement.Timestamp,
                      Y: GetTimeseriesValue(
                        measurement,
                        dataset.TimeseriesChartDatasetPart.Value.Property
                      )
                    )
                )
                .ToList()
            )
        )
        .ToList()
    );
  }

  public AbbChartProvider(ITimeseriesClient client)
  {
    _client = client;
  }

  private readonly ITimeseriesClient _client;
}
