using Mess.Cms;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class AbbPushHandler
  : JsonIotPushHandler<AbbIotDeviceItem, AbbPushRequest>
{
  private readonly IContentManager _contentManager;
  private readonly IOzdsTimeseriesClient _measurementClient;

  public AbbPushHandler(
    IOzdsTimeseriesClient measurementClient,
    IContentManager contentManager)
  {
    _contentManager = contentManager;
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
    var newMeasurement = MakeMeasurement(deviceId, tenant, timestamp, contentItem, request);
    _measurementClient.AddAbbMeasurement(newMeasurement);
    contentItem.Alter(
      abbIotDevice => abbIotDevice.AbbIotDevicePart,
      abbIotDevicePart =>
      {
        abbIotDevicePart.LatestImport = newMeasurement.ActiveEnergyImportTotal_Wh;
      });
    _contentManager.UpdateAsync(contentItem).RunSynchronously();
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    AbbIotDeviceItem contentItem,
    AbbPushRequest request
  )
  {
    var newMeasurement = MakeMeasurement(deviceId, tenant, timestamp, contentItem, request);
    await _measurementClient.AddAbbMeasurementAsync(newMeasurement);
    contentItem.Alter(
      abbIotDevice => abbIotDevice.AbbIotDevicePart,
      abbIotDevicePart =>
      {
        abbIotDevicePart.LatestImport = newMeasurement.ActiveEnergyImportTotal_Wh;
      });
    await _contentManager.UpdateAsync(contentItem);
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
      request.ActivePowerL1_W,
      request.ActivePowerL2_W,
      request.ActivePowerL3_W,
      request.ReactivePowerL1_VAR,
      request.ReactivePowerL2_VAR,
      request.ReactivePowerL3_VAR,
      request.ActivePowerImportL1_Wh,
      request.ActivePowerImportL2_Wh,
      request.ActivePowerImportL3_Wh,
      request.ActivePowerExportL1_Wh,
      request.ActivePowerExportL2_Wh,
      request.ActivePowerExportL3_Wh,
      request.ReactivePowerImportL1_VARh,
      request.ReactivePowerImportL2_VARh,
      request.ReactivePowerImportL3_VARh,
      request.ReactivePowerExportL1_VARh,
      request.ReactivePowerExportL2_VARh,
      request.ReactivePowerExportL3_VARh,
      request.ActiveEnergyImportTotal_Wh,
      request.ActiveEnergyExportTotal_Wh,
      request.ReactiveEnergyImportTotal_VARh,
      request.ReactiveEnergyExportTotal_VARh
    );
  }
}
