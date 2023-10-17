using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Chart;

public class AbbChartProvider : ChartFactory<AbbMeasurementDeviceItem>
{
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
      nameof(AbbMeasurement.Energy),
      nameof(AbbMeasurement.LowEnergy),
      nameof(AbbMeasurement.HighEnergy),
      nameof(AbbMeasurement.Power),
    };

  protected override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    AbbMeasurementDeviceItem abbMeasurementDevice,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var now = DateTimeOffset.UtcNow;
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

  public AbbChartProvider(IOzdsTimeseriesClient client)
  {
    _client = client;
  }

  private readonly IOzdsTimeseriesClient _client;
}
