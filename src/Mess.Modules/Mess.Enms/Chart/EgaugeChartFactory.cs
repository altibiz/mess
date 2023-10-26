using Mess.Enms.Abstractions.Timeseries;
using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.Enms.Abstractions.Models;

namespace Mess.Enms.Chart;

public class EgaugeChartFactory : ChartFactory<EgaugeIotDeviceItem>
{
  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EgaugeMeasurement.Power),
      nameof(EgaugeMeasurement.Voltage)
    };

  protected override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    EgaugeIotDeviceItem egaugeIotDevice,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetEgaugeMeasurementsAsync(
      egaugeIotDevice.IotDevicePart.Value.DeviceId.Text,
      now.Subtract(chart.TimeseriesChartPart.Value.History.Value.ToTimeSpan()),
      now
    );

    return new TimeseriesChartDescriptor(
      RefreshInterval: (decimal)
        chart.TimeseriesChartPart.Value.RefreshInterval.Value
          .ToTimeSpan()
          .TotalMilliseconds,
      History: (decimal)
        chart.TimeseriesChartPart.Value.History.Value
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

  public EgaugeChartFactory(IEnmsTimeseriesClient client)
  {
    _client = client;
  }

  private readonly IEnmsTimeseriesClient _client;
}
