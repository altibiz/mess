using Mess.Cms;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class SchneiderPushHandler
  : JsonIotPushHandler<SchneiderIotDeviceItem, SchneiderPushRequest>
{
  private readonly IContentManager _contentManager;
  private readonly IOzdsTimeseriesClient _measurementClient;

  public SchneiderPushHandler(
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
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  )
  {
    var newMeasurement = MakeMeasurement(deviceId, tenant, timestamp, contentItem, request);
    _measurementClient.AddSchneiderMeasurement(
      newMeasurement
    );
    contentItem.Alter(
      schneiderIotDevice => schneiderIotDevice.OzdsIotDevicePart,
      part =>
      {
        part.LatestImport = newMeasurement.ActiveEnergyImportTotal_Wh;
      });
    _contentManager.UpdateAsync(contentItem).RunSynchronously();
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  )
  {
    var newMeasurement = MakeMeasurement(deviceId, tenant, timestamp, contentItem, request);
    await _measurementClient.AddSchneiderMeasurementAsync(
      newMeasurement
    );
    contentItem.Alter(
      schneiderIotDevice => schneiderIotDevice.OzdsIotDevicePart,
      part =>
      {
        part.LatestImport = newMeasurement.ActiveEnergyImportTotal_Wh;
      });
    await _contentManager.UpdateAsync(contentItem);
  }

  private static SchneiderMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    SchneiderPushRequest request
  )
  {
    return new SchneiderMeasurement(
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
      request.ReactivePowerTotal_VAR,
      request.ApparentPowerTotal_VA,
      request.ActiveEnergyImportL1_Wh,
      request.ActiveEnergyImportL2_Wh,
      request.ActiveEnergyImportL3_Wh,
      request.ActiveEnergyImportTotal_Wh,
      request.ActiveEnergyExportTotal_Wh,
      request.ReactiveEnergyImportTotal_VARh,
      request.ReactiveEnergyExportTotal_VARh
    );
  }
}
