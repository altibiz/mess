using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class AbbPushHandler
  : JsonIotPushHandler<AbbIotDeviceItem, AbbPushRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    AbbIotDeviceItem contentItem,
    AbbPushRequest request
  ) =>
    _measurementClient.AddAbbMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    AbbIotDeviceItem contentItem,
    AbbPushRequest request
  ) =>
    await _measurementClient.AddAbbMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  private AbbMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    AbbPushRequest request
  ) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: timestamp,
      VoltageL1_V: request.VoltageL1_V,
      VoltageL2_V: request.VoltageL2_V,
      VoltageL3_V: request.VoltageL3_V,
      CurrentL1_A: request.CurrentL1_A,
      CurrentL2_A: request.CurrentL2_A,
      CurrentL3_A: request.CurrentL3_A,
      ActivePowerTotal_W: request.ActivePowerTotal_W,
      ActivePowerL1_W: request.ActivePowerL1_W,
      ActivePowerL2_W: request.ActivePowerL2_W,
      ActivePowerL3_W: request.ActivePowerL3_W,
      ReactivePowerTotal_VAR: request.ReactivePowerTotal_VAR,
      ReactivePowerL1_VAR: request.ReactivePowerL1_VAR,
      ReactivePowerL2_VAR: request.ReactivePowerL2_VAR,
      ReactivePowerL3_VAR: request.ReactivePowerL3_VAR,
      ApparentPowerTotal_VA: request.ApparentPowerTotal_VA,
      ApparentPowerL1_VA: request.ApparentPowerL1_VA,
      ApparentPowerL2_VA: request.ApparentPowerL2_VA,
      ApparentPowerL3_VA: request.ApparentPowerL3_VA,
      PowerFactorTotal: request.PowerFactorTotal,
      PowerFactorL1: request.PowerFactorL1,
      PowerFactorL2: request.PowerFactorL2,
      PowerFactorL3: request.PowerFactorL3,
      ActiveEnergyImportTotal_kWh: request.ActiveEnergyImportTotal_kWh,
      ActiveEnergyExportTotal_kWh: request.ActiveEnergyExportTotal_kWh,
      ActiveEnergyNetTotal_kWh: request.ActiveEnergyNetTotal_kWh,
      ActiveEnergyImportTariff1_kWh: request.ActiveEnergyImportTariff1_kWh,
      ActiveEnergyImportTariff2_kWh: request.ActiveEnergyImportTariff2_kWh,
      ActiveEnergyExportTariff1_kWh: request.ActiveEnergyExportTariff1_kWh,
      ActiveEnergyExportTariff2_kWh: request.ActiveEnergyExportTariff2_kWh
    );

  public AbbPushHandler(IOzdsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  private readonly IOzdsTimeseriesClient _measurementClient;
}
