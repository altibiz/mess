using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Chart;

public class SchneiderChartProvider : ChartFactory<SchneiderIotDeviceItem>
{
  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(SchneiderMeasurement.CurrentL1),
      nameof(SchneiderMeasurement.CurrentL2),
      nameof(SchneiderMeasurement.CurrentL3),
      nameof(SchneiderMeasurement.VoltageL1),
      nameof(SchneiderMeasurement.VoltageL2),
      nameof(SchneiderMeasurement.VoltageL3),
      nameof(SchneiderMeasurement.ActivePowerL1),
      nameof(SchneiderMeasurement.ActivePowerL2),
      nameof(SchneiderMeasurement.ActivePowerL3),
      nameof(SchneiderMeasurement.ReactivePowerL1),
      nameof(SchneiderMeasurement.ReactivePowerL2),
      nameof(SchneiderMeasurement.ReactivePowerL3),
      nameof(SchneiderMeasurement.ApparentPowerL1),
      nameof(SchneiderMeasurement.ApparentPowerL2),
      nameof(SchneiderMeasurement.ApparentPowerL3),
      nameof(SchneiderMeasurement.PowerFactorL1),
      nameof(SchneiderMeasurement.PowerFactorL2),
      nameof(SchneiderMeasurement.PowerFactorL3),
      nameof(SchneiderMeasurement.Energy),
      nameof(SchneiderMeasurement.LowEnergy),
      nameof(SchneiderMeasurement.HighEnergy),
      nameof(SchneiderMeasurement.Power),
    };

  protected override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    SchneiderIotDeviceItem abbIotDevice,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetSchneiderMeasurementsAsync(
      abbIotDevice.IotDevicePart.Value.DeviceId.Text,
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

  public SchneiderChartProvider(IOzdsTimeseriesClient client)
  {
    _client = client;
  }

  private readonly IOzdsTimeseriesClient _client;
}
