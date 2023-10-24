using Mess.Iot.Abstractions.Services;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;

namespace Mess.Eor.Iot;

public class EorPushHandler
  : JsonIotPushHandler<EorIotDeviceItem, EorPushRequest>
{
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

  public EorPushHandler(IEorTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;
}
