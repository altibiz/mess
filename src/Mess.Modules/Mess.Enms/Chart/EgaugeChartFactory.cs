using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Enms.Abstractions.Models;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Fields.Abstractions;

namespace Mess.Enms.Chart;

public class EgaugeChartFactory : ChartFactory<EgaugeIotDeviceItem>
{
  private readonly IEnmsTimeseriesClient _client;

  public EgaugeChartFactory(IEnmsTimeseriesClient client)
  {
    _client = client;
  }

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EgaugeMeasurement.Power),
      nameof(EgaugeMeasurement.Voltage)
    };

  protected override async Task<TimeseriesChartDescriptor?>
    CreateTimeseriesChartAsync(
      EgaugeIotDeviceItem metadata,
      TimeseriesChartItem chart,
      IEnumerable<TimeseriesChartDatasetItem> datasets
    )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetEgaugeMeasurementsAsync(
      metadata.IotDevicePart.Value.DeviceId.Text,
      now.Subtract(chart.TimeseriesChartPart.Value.History.Value.ToTimeSpan()),
      now
    );

    return new TimeseriesChartDescriptor(
      (decimal)
      chart.TimeseriesChartPart.Value.RefreshInterval.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      (decimal)
      chart.TimeseriesChartPart.Value.History.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      datasets
        .Select(
          dataset =>
            new TimeseriesChartDatasetDescriptor(
              dataset.TimeseriesChartDatasetPart.Value.Label.Text,
              dataset.TimeseriesChartDatasetPart.Value.Color.Value,
              measurements
                .Select(
                  measurement =>
                    new TimeseriesChartDatapointDescriptor(
                      measurement.Timestamp,
                      GetTimeseriesValue(
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
}
