using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.Abstractions.Services;
using Mess.Fields.Abstractions;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Chart;

public class SchneiderChartFactory : ChartFactory<SchneiderIotDeviceItem>
{
  private readonly IOzdsTimeseriesClient _client;

  public SchneiderChartFactory(IOzdsTimeseriesClient client)
  {
    _client = client;
  }

  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(SchneiderMeasurement.VoltageL1_V),
      nameof(SchneiderMeasurement.VoltageL2_V),
      nameof(SchneiderMeasurement.VoltageL3_V),
      nameof(SchneiderMeasurement.VoltageAvg_V),
      nameof(SchneiderMeasurement.CurrentL1_A),
      nameof(SchneiderMeasurement.CurrentL2_A),
      nameof(SchneiderMeasurement.CurrentL3_A),
      nameof(SchneiderMeasurement.CurrentAvg_A),
      nameof(SchneiderMeasurement.ActivePowerL1_kW),
      nameof(SchneiderMeasurement.ActivePowerL2_kW),
      nameof(SchneiderMeasurement.ActivePowerL3_kW),
      nameof(SchneiderMeasurement.ActivePowerTotal_kW),
      nameof(SchneiderMeasurement.ReactivePowerTotal_kVAR),
      nameof(SchneiderMeasurement.ApparentPowerTotal_kVA),
      nameof(SchneiderMeasurement.PowerFactorTotal),
      nameof(SchneiderMeasurement.ActiveEnergyImportTotal_Wh),
      nameof(SchneiderMeasurement.ActiveEnergyExportTotal_Wh),
      nameof(SchneiderMeasurement.ActiveEnergyImportRateA_Wh),
      nameof(SchneiderMeasurement.ActiveEnergyImportRateB_Wh)
    };

  protected override async Task<TimeseriesChartDescriptor?>
    CreateTimeseriesChartAsync(
      SchneiderIotDeviceItem metadata,
      TimeseriesChartItem chart,
      IEnumerable<TimeseriesChartDatasetItem> datasets
    )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetSchneiderMeasurementsAsync(
      metadata.IotDevicePart.Value.DeviceId.Text,
      now.Subtract(chart.TimeseriesChartPart.Value.History.Value.ToTimeSpan()),
      now
    );

    return new TimeseriesChartDescriptor(
      (decimal)chart.TimeseriesChartPart.Value.RefreshInterval.Value
        .ToTimeSpan()
        .TotalMilliseconds,
      (decimal)chart.TimeseriesChartPart.Value.History.Value
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
