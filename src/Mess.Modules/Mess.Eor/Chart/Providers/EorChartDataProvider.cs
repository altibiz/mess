using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Models;
using Mess.OrchardCore;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.Chart.Providers;

public class EorChartProvider : ChartProvider
{
  public const string ChartProviderId = "Eor";

  public override string Id => ChartProviderId;

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EorMeasurement.Voltage),
      nameof(EorMeasurement.Current),
      nameof(EorStatus.Mode)
    };

  public override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    ContentItem metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var eorMeasurementDevice = metadata.AsContent<EorMeasurementDeviceItem>();
    if (eorMeasurementDevice == null)
    {
      return default;
    }

    var now = DateTime.UtcNow;
    var (statuses, measurements) = await _client.GetEorDataAsync(
      eorMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text,
      now.Subtract(chart.TimeseriesChartPart.Value.HistorySpan),
      now
    );

    return new TimeseriesChartDescriptor(
      RefreshInterval: chart.TimeseriesChartPart.Value.RefreshIntervalSpan,
      History: chart.TimeseriesChartPart.Value.HistorySpan,
      Datasets: datasets
        .Select(
          dataset =>
            new TimeseriesChartDatasetDescriptor(
              Label: dataset.TimeseriesChartDatasetPart.Value.Label.Text,
              Color: dataset.TimeseriesChartDatasetPart.Value.Color.Value,
              Datapoints: (
                (
                  ContainsTimeseriesProperty<EorStatus>(
                    dataset.TimeseriesChartDatasetPart.Value.Property
                  )
                    ? statuses as IEnumerable<object>
                    : measurements as IEnumerable<object>
                )
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
