using Mess.Iot.Abstractions.Pushing;
using Mess.Ozds.Abstractions.Client;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Pushing;

public class AbbPushHandler : JsonMeasurementDevicePushHandler<AbbPushRequest>
{
  public const string PushContentType = "AbbMeasurementDevice";

  public override string ContentType => PushContentType;

  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    AbbPushRequest request
  ) =>
    _measurementClient.AddAbbMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem contentItem,
    AbbPushRequest request
  ) =>
    await _measurementClient.AddAbbMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  private AbbMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    AbbPushRequest request
  ) =>
    new(
      Tenant: tenant,
      DeviceId: deviceId,
      Timestamp: timestamp,
      CurrentL1: request.CurrentL2,
      CurrentL2: request.CurrentL2,
      CurrentL3: request.CurrentL3,
      VoltageL1: request.VoltageL1,
      VoltageL2: request.VoltageL2,
      VoltageL3: request.VoltageL3,
      ActivePowerL1: request.ActivePowerL1,
      ActivePowerL2: request.ActivePowerL2,
      ActivePowerL3: request.ActivePowerL3,
      ReactivePowerL1: request.ReactivePowerL1,
      ReactivePowerL2: request.ReactivePowerL2,
      ReactivePowerL3: request.ReactivePowerL3,
      ApparentPowerL1: request.ApparentPowerL1,
      ApparentPowerL2: request.ApparentPowerL2,
      ApparentPowerL3: request.ApparentPowerL3,
      PowerFactorL1: request.PowerFactorL1,
      PowerFactorL2: request.PowerFactorL2,
      PowerFactorL3: request.PowerFactorL3,
      Energy: request.Energy,
      LowEnergy: request.LowEnergy,
      HighEnergy: request.HighEnergy,
      Power: request.Power
    );

  public AbbPushHandler(
    ILogger<AbbPushHandler> logger,
    IOzdsClient measurementClient
  )
    : base(logger)
  {
    _measurementClient = measurementClient;
  }

  private readonly IOzdsClient _measurementClient;
}
