using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Descriptors;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Models;
using Mess.Fields.Abstractions;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Chart;

public class AbbChartFactory : ChartFactory<AbbIotDeviceItem>
{
  public override IEnumerable<string> TimeseriesChartDatasetProperties =>
    new[]
    {
      nameof(AbbMeasurement.VoltageL1_V),
      nameof(AbbMeasurement.VoltageL2_V),
      nameof(AbbMeasurement.VoltageL3_V),
      nameof(AbbMeasurement.CurrentL1_A),
      nameof(AbbMeasurement.CurrentL2_A),
      nameof(AbbMeasurement.CurrentL3_A),
      nameof(AbbMeasurement.ActivePowerTotal_W),
      nameof(AbbMeasurement.ActivePowerL1_W),
      nameof(AbbMeasurement.ActivePowerL2_W),
      nameof(AbbMeasurement.ActivePowerL3_W),
      nameof(AbbMeasurement.ReactivePowerTotal_VAR),
      nameof(AbbMeasurement.ReactivePowerL1_VAR),
      nameof(AbbMeasurement.ReactivePowerL2_VAR),
      nameof(AbbMeasurement.ReactivePowerL3_VAR),
      nameof(AbbMeasurement.ApparentPowerTotal_VA),
      nameof(AbbMeasurement.ApparentPowerL1_VA),
      nameof(AbbMeasurement.ApparentPowerL2_VA),
      nameof(AbbMeasurement.ApparentPowerL3_VA),
      nameof(AbbMeasurement.PowerFactorTotal),
      nameof(AbbMeasurement.PowerFactorL1),
      nameof(AbbMeasurement.PowerFactorL2),
      nameof(AbbMeasurement.PowerFactorL3),
      nameof(AbbMeasurement.ActiveEnergyImportTotal_kWh),
      nameof(AbbMeasurement.ActiveEnergyExportTotal_kWh),
      nameof(AbbMeasurement.ActiveEnergyNetTotal_kWh),
      nameof(AbbMeasurement.ActiveEnergyImportTariff1_kWh),
      nameof(AbbMeasurement.ActiveEnergyImportTariff2_kWh),
      nameof(AbbMeasurement.ActiveEnergyExportTariff1_kWh),
      nameof(AbbMeasurement.ActiveEnergyExportTariff2_kWh)
    };

  protected override async Task<TimeseriesChartDescriptor?> CreateTimeseriesChartAsync(
    AbbIotDeviceItem abbIotDevice,
    TimeseriesChartItem chart,
    IEnumerable<TimeseriesChartDatasetItem> datasets
  )
  {
    var now = DateTimeOffset.UtcNow;
    var measurements = await _client.GetAbbMeasurementsAsync(
      abbIotDevice.IotDevicePart.Value.DeviceId.Text,
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

  public AbbChartFactory(IOzdsTimeseriesClient client)
  {
    _client = client;
  }

  private readonly IOzdsTimeseriesClient _client;
}
