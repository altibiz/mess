using Mess.MeasurementDevice.Abstractions.Client;
using OrchardCore.ContentManagement;
using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Descriptors;
using Mess.MeasurementDevice.Abstractions.Models;

namespace Mess.MeasurementDevice.Chart.Providers;

public class EgaugeChartDataProvider : IChartDataProvider
{
  public const string ProviderId = "Egauge";

  public string Id => ProviderId;

  public async Task<ChartDescriptor?> CreateChartAsync(
    ContentItem metadata,
    ContentItem chart
  )
  {
    return await Task.FromResult(
      new TimeseriesChartDescriptor(
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(5),
        new List<TimeseriesChartDatasetDescriptor>()
      )
    );
  }

  public IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(EgaugeMeasurementModel.Power),
      nameof(EgaugeMeasurementModel.Voltage)
    };

  public EgaugeChartDataProvider(IMeasurementClient client)
  {
    _client = client;
  }

  private readonly IMeasurementClient _client;
}
