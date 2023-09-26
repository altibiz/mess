using Mess.Iot.Abstractions.Pushing;
using Microsoft.Extensions.Logging;
using Mess.Eor.Abstractions.Client;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Pushing;

public class EorPushHandler : JsonMeasurementDevicePushHandler<EorPushRequest>
{
  public const string PushContentType = "EorMeasurementDevice";

  public override string ContentType => PushContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
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
    DateTime timestamp,
    ContentItem contentItem,
    EorPushRequest request
  )
  {
    await _measurementClient.AddEorMeasurementAsync(
      request.ToMeasurement(deviceId, tenant)
    );
  }

  public EorPushHandler(
    ILogger<JsonMeasurementDevicePushHandler<EorMeasurement>> logger,
    IEorTimeseriesClient measurementClient
  )
    : base(logger)
  {
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;
}
