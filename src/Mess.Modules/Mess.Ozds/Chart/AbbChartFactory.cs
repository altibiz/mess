using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Fields.Abstractions;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Chart;

public class AbbChartFactory : ChartFactory<AbbIotDeviceItem>
{
  private readonly IOzdsTimeseriesClient _client;

  public AbbChartFactory(IOzdsTimeseriesClient client)
  {
    _client = client;
  }

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(AbbMeasurement.VoltageL1_V),
      nameof(AbbMeasurement.VoltageL2_V),
      nameof(AbbMeasurement.VoltageL3_V),
      nameof(AbbMeasurement.CurrentL1_A),
      nameof(AbbMeasurement.CurrentL2_A),
      nameof(AbbMeasurement.CurrentL3_A),
      nameof(AbbMeasurement.ActivePowerL1_W),
      nameof(AbbMeasurement.ActivePowerL2_W),
      nameof(AbbMeasurement.ActivePowerL3_W),
      nameof(AbbMeasurement.ReactivePowerL1_VAR),
      nameof(AbbMeasurement.ReactivePowerL2_VAR),
      nameof(AbbMeasurement.ReactivePowerL3_VAR),
      nameof(AbbMeasurement.ActivePowerImportL1_Wh),
      nameof(AbbMeasurement.ActivePowerImportL2_Wh),
      nameof(AbbMeasurement.ActivePowerImportL3_Wh),
      nameof(AbbMeasurement.ActivePowerExportL1_Wh),
      nameof(AbbMeasurement.ActivePowerExportL2_Wh),
      nameof(AbbMeasurement.ActivePowerExportL3_Wh),
      nameof(AbbMeasurement.ReactivePowerImportL1_VARh),
      nameof(AbbMeasurement.ReactivePowerImportL2_VARh),
      nameof(AbbMeasurement.ReactivePowerImportL3_VARh),
      nameof(AbbMeasurement.ReactivePowerExportL1_VARh),
      nameof(AbbMeasurement.ReactivePowerExportL2_VARh),
      nameof(AbbMeasurement.ReactivePowerExportL3_VARh),
      nameof(AbbMeasurement.ActiveEnergyImportTotal_Wh),
      nameof(AbbMeasurement.ActiveEnergyExportTotal_Wh),
      nameof(AbbMeasurement.ReactiveEnergyImportTotal_VARh),
      nameof(AbbMeasurement.ReactiveEnergyExportTotal_VARh)
    };

  protected override async Task<TimeseriesChartDescriptor?>
    CreateTimeseriesChartAsync(
      AbbIotDeviceItem metadata,
      TimeseriesChartItem chart,
      IEnumerable<TimeseriesChartDatasetItem> datasets
    )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetAbbMeasurementsAsync(
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
