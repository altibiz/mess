using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class SchneiderPushHandler
  : JsonIotPushHandler<SchneiderIotDeviceItem, SchneiderPushRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  ) =>
    _measurementClient.AddSchneiderMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  ) =>
    await _measurementClient.AddSchneiderMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  private static SchneiderMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    SchneiderPushRequest request
  ) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: timestamp,
      VoltageL1_V: request.VoltageL1_V,
      VoltageL2_V: request.VoltageL2_V,
      VoltageL3_V: request.VoltageL3_V,
      VoltageAvg_V: request.VoltageAvg_V,
      CurrentL1_A: request.CurrentL1_A,
      CurrentL2_A: request.CurrentL2_A,
      CurrentL3_A: request.CurrentL3_A,
      CurrentAvg_A: request.CurrentAvg_A,
      ActivePowerL1_kW: request.ActivePowerL1_kW,
      ActivePowerL2_kW: request.ActivePowerL2_kW,
      ActivePowerL3_kW: request.ActivePowerL3_kW,
      ActivePowerTotal_kW: request.ActivePowerTotal_kW,
      ReactivePowerTotal_kVAR: request.ReactivePowerTotal_kVAR,
      ApparentPowerTotal_kVA: request.ApparentPowerTotal_kVA,
      PowerFactorTotal: request.PowerFactorTotal,
      ActiveEnergyImportTotal_Wh: request.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotal_Wh: request.ActiveEnergyExportTotal_Wh,
      ActiveEnergyImportRateA_Wh: request.ActiveEnergyImportRateA_Wh,
      ActiveEnergyImportRateB_Wh: request.ActiveEnergyImportRateB_Wh
    );

  public SchneiderPushHandler(IOzdsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  private readonly IOzdsTimeseriesClient _measurementClient;
}
