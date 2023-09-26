using Mess.Iot.Abstractions.Client;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Descriptors;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Models;
using Mess.Iot.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.OrchardCore;

namespace Mess.Iot.Chart;

public class EgaugeChartProvider : ChartProvider
{
  public const string ChartContentType = "EgaugeMeasurementDevice";

  public override string ContentType => ChartContentType;

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EgaugeMeasurement.Power),
      nameof(EgaugeMeasurement.Voltage)
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
      egaugeMeasurementDevice.MeasurementDevicePart.Value.DeviceId.Text,
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

  public EgaugeChartProvider(ITimeseriesClient client)
  {
    _client = client;
  }

  private readonly ITimeseriesClient _client;
}
