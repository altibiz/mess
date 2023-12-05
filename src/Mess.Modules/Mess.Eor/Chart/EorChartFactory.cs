using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Fields.Abstractions;

namespace Mess.Eor.Chart;

public class EorChartProvider : ChartFactory<EorIotDeviceItem>
{
  private readonly IEorTimeseriesClient _client;

  public EorChartProvider(IEorTimeseriesClient client)
  {
    _client = client;
  }

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EorMeasurement.Voltage),
      nameof(EorMeasurement.Current),
      nameof(EorStatus.Mode)
    };

  protected override async Task<TimeseriesChartDescriptor?>
    CreateTimeseriesChartAsync(
      EorIotDeviceItem metadata,
      TimeseriesChartItem chart,
      IEnumerable<TimeseriesChartDatasetItem> datasets
    )
  {
    var now = DateTimeOffset.UtcNow;
    var (statuses, measurements) = await _client.GetEorDataAsync(
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
              (
                ContainsTimeseriesProperty<EorStatus>(
                  dataset.TimeseriesChartDatasetPart.Value.Property
                )
                  ? statuses
                  : measurements as IEnumerable<object>
              )
              .Select(
                measurement =>
                  new TimeseriesChartDatapointDescriptor(
                    GetTimeseriesTimestamp(measurement),
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
