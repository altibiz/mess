using Mess.MeasurementDevice.Abstractions.Client;
using Mess.MeasurementDevice.Abstractions.Pushing;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Pushing;

public class AbbPushHandler : JsonMeasurementDevicePushHandler<AbbPushRequest>
{
  public const string PushContentType = "AbbMeasurementDevice";

  public override string ContentType => PushContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    AbbPushRequest request
  ) =>
    _measurementClient.AddAbbMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem contentItem,
    AbbPushRequest request
  ) =>
    await _measurementClient.AddAbbMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  private AbbMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTime timestamp,
    ContentItem _,
    AbbPushRequest request
  ) =>
    new AbbMeasurement(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: timestamp,
      CurrentL1: request.currentL1,
      CurrentL2: request.currentL2,
      CurrentL3: request.currentL3,
      VoltageL1: request.voltageL1,
      VoltageL2: request.voltageL2,
      VoltageL3: request.voltageL3,
      ActivePowerL1: request.activePowerL1,
      ActivePowerL2: request.activePowerL2,
      ActivePowerL3: request.activePowerL3,
      ReactivePowerL1: request.reactivePowerL1,
      ReactivePowerL2: request.reactivePowerL2,
      ReactivePowerL3: request.reactivePowerL3,
      ApparentPowerL1: request.apparentPowerL1,
      ApparentPowerL2: request.apparentPowerL2,
      ApparentPowerL3: request.apparentPowerL3,
      PowerFactorL1: request.powerFactorL1,
      PowerFactorL2: request.powerFactorL2,
      PowerFactorL3: request.powerFactorL3
    );

  public AbbPushHandler(
    ILogger<AbbPushHandler> logger,
    ITimeseriesClient measurementClient
  )
    : base(logger)
  {
    _measurementClient = measurementClient;
  }

  private readonly ITimeseriesClient _measurementClient;
}
