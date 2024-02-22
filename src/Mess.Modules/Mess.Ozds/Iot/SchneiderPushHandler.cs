using Mess.Cms;
using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class SchneiderPushHandler
  : JsonIotPushHandler<SchneiderIotDeviceItem, SchneiderPushRequest>
{
  private readonly IOzdsTimeseriesClient _measurementClient;

  public SchneiderPushHandler(
    IOzdsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  )
  {
    var newMeasurement = MakeMeasurement(deviceId, timestamp, contentItem, request);
    _measurementClient.AddSchneiderMeasurement(
      newMeasurement
    );
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  )
  {
    var newMeasurement = MakeMeasurement(deviceId, timestamp, contentItem, request);
    await _measurementClient.AddSchneiderMeasurementAsync(
      newMeasurement
    );
  }

  protected override void HandleBulk(BulkIotJsonPushRequest<SchneiderIotDeviceItem, SchneiderPushRequest>[] requests)
  {
    var newMeasurement = requests
      .Select(request =>
        MakeMeasurement(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          request.Request))
      .ToArray();
    _measurementClient.AddBulkSchneiderMeasurement(
      newMeasurement
    );
  }

  protected override async Task HandleBulkAsync(BulkIotJsonPushRequest<SchneiderIotDeviceItem, SchneiderPushRequest>[] requests)
  {
    var newMeasurement = requests
      .Select(request =>
        MakeMeasurement(
          request.DeviceId,
          request.Timestamp,
          request.ContentItem,
          request.Request))
      .ToArray();
    await _measurementClient.AddBulkSchneiderMeasurementAsync(
      newMeasurement
    );
  }

  private static SchneiderMeasurement MakeMeasurement(
    string source,
    DateTimeOffset timestamp,
    ContentItem _,
    SchneiderPushRequest request
  )
  {
    return new SchneiderMeasurement(
      source,
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
