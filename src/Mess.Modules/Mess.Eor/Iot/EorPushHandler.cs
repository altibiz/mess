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
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorPushRequest request
  )
  {
    _measurementClient.AddEorMeasurement(
      request.ToMeasurement(deviceId, tenant)
    );
  }

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    EorIotDeviceItem contentItem,
    EorPushRequest request
  )
  {
    await _measurementClient.AddEorMeasurementAsync(
      request.ToMeasurement(deviceId, tenant)
    );
  }
}
