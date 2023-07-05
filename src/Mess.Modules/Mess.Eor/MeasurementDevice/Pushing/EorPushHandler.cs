using Mess.MeasurementDevice.Abstractions.Pushing;
using Microsoft.Extensions.Logging;
using Mess.Eor.Abstractions.Client;
using OrchardCore.ContentManagement;

namespace Mess.Eor.MeasurementDevice.Pushing;

public class EorPushHandler : JsonPushHandler<EorMeasurement>
{
  public const string PushHandlerId = "eor";

  public override string Id => PushHandlerId;

  protected override void Handle(
    string deviceId,
    ContentItem contentItem,
    EorMeasurement measurement
  )
  {
    _measurementClient.AddEorMeasurement(measurement);
  }

  protected override async Task HandleAsync(
    string deviceId,
    ContentItem contentItem,
    EorMeasurement measurement
  )
  {
    await _measurementClient.AddEorMeasurementAsync(measurement);
  }

  public EorPushHandler(
    ILogger<JsonPushHandler<EorMeasurement>> logger,
    IEorTimeseriesClient measurementClient
  )
    : base(logger)
  {
    _measurementClient = measurementClient;
  }

  private readonly IEorTimeseriesClient _measurementClient;
}
