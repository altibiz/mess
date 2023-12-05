using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class AbbPushHandler
  : JsonIotPushHandler<AbbIotDeviceItem, AbbPushRequest>
{
  private readonly IOzdsTimeseriesClient _measurementClient;

  public AbbPushHandler(IOzdsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    AbbIotDeviceItem contentItem,
    AbbPushRequest request
  )
  {
    _measurementClient.AddAbbMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    AbbIotDeviceItem contentItem,
    AbbPushRequest request
  )
  {
    await _measurementClient.AddAbbMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );
  }

  private static AbbMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    AbbPushRequest request
  )
  {
    return new AbbMeasurement(
      tenant,
      deviceId,
      timestamp,
      request.VoltageL1_V,
      request.VoltageL2_V,
      request.VoltageL3_V,
      request.CurrentL1_A,
      request.CurrentL2_A,
      request.CurrentL3_A,
      request.ActivePowerTotal_W,
      request.ActivePowerL1_W,
      request.ActivePowerL2_W,
      request.ActivePowerL3_W,
      request.ReactivePowerTotal_VAR,
      request.ReactivePowerL1_VAR,
      request.ReactivePowerL2_VAR,
      request.ReactivePowerL3_VAR,
      request.ApparentPowerTotal_VA,
      request.ApparentPowerL1_VA,
      request.ApparentPowerL2_VA,
      request.ApparentPowerL3_VA,
      request.PowerFactorTotal,
      request.PowerFactorL1,
      request.PowerFactorL2,
      request.PowerFactorL3,
      request.ActiveEnergyImportTotal_kWh,
      request.ActiveEnergyExportTotal_kWh,
      request.ActiveEnergyNetTotal_kWh,
      request.ActiveEnergyImportTariff1_kWh,
      request.ActiveEnergyImportTariff2_kWh,
      request.ActiveEnergyExportTariff1_kWh,
      request.ActiveEnergyExportTariff2_kWh
    );
  }
}
