using Mess.MeasurementDevice.Abstractions.Client;
using Mess.Chart.Abstractions.Providers;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Models;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.OrchardCore;

namespace Mess.MeasurementDevice.Chart.Providers;

public class EgaugeChartDataProvider : ChartDataProvider
{
  public const string ProviderId = "Egauge";

  public override string Id => ProviderId;

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(DispatchedEgaugeMeasurement.Power),
      nameof(DispatchedEgaugeMeasurement.Voltage)
    };

  public override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    ContentItem metadata,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var egaugeMeasurementDevice =
      metadata.AsContent<EgaugeMeasurementDeviceItem>();
    if (egaugeMeasurementDevice == null)
    {
      return default;
    }

    var now = DateTime.UtcNow;
    var measurements = await _client.GetEgaugeMeasurementsAsync(
      egaugeMeasurementDevice.EgaugeMeasurementDevicePart.Value.DeviceId.Text,
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

  public EgaugeChartDataProvider(IMeasurementClient client)
  {
    _client = client;
  }

  private readonly IMeasurementClient _client;
}
