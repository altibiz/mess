using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Iot.Abstractions.Services;

namespace Mess.Eor.Iot;

public class EorPushHandler
  : JsonIotPushHandler<EorIotDeviceItem, EorPushRequest>
{
  private readonly IEorTimeseriesClient _measurementClient;

  public EorPushHandler(IEorTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  protected override void Handle(
    string deviceId,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorPushRequest request
  )
  {
    _measurementClient.AddEorMeasurement(
      request.ToMeasurement(deviceId)
    );
  }

  protected override async Task HandleAsync(
    string deviceId,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorPushRequest request
  )
  {
    await _measurementClient.AddEorMeasurementAsync(
      request.ToMeasurement(deviceId)
    );
  }

  protected override void HandleBulk(BulkIotJsonPushRequest<EorIotDeviceItem, EorPushRequest>[] requests)
  {
    foreach (var request in requests)
    {
      _measurementClient.AddEorMeasurement(
        request.Request.ToMeasurement(request.DeviceId)
      );
    }
  }

  protected override async Task HandleBulkAsync(BulkIotJsonPushRequest<EorIotDeviceItem, EorPushRequest>[] requests)
  {
    foreach (var request in requests)
    {
      await _measurementClient.AddEorMeasurementAsync(
        request.Request.ToMeasurement(request.DeviceId)
      );
    }
  }
}
