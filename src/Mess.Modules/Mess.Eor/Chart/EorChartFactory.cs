using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Descriptors;
using Mess.Fields.Abstractions;
using Mess.Chart.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.Chart;

public class EorChartProvider : ChartFactory<EorIotDeviceItem>
{
  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EorMeasurement.Voltage),
      nameof(EorMeasurement.Current),
      nameof(EorStatus.Mode)
    };

  protected override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
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
              Datapoints: 
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
                      X: GetTimeseriesTimestamp(measurement),
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

  public EorChartProvider(IEorTimeseriesClient client)
  {
    _client = client;
  }

  private readonly IEorTimeseriesClient _client;
}
