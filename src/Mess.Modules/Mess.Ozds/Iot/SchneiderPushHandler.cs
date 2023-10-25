using Mess.Iot.Abstractions.Services;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using OrchardCore.ContentManagement;

namespace Mess.Ozds.Iot;

public class SchneiderPushHandler
  : JsonIotPushHandler<SchneiderIotDeviceItem, SchneiderPushRequest>
{
  protected override void Handle(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  ) =>
    _measurementClient.AddSchneiderMeasurement(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  protected override async Task HandleAsync(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    SchneiderIotDeviceItem contentItem,
    SchneiderPushRequest request
  ) =>
    await _measurementClient.AddSchneiderMeasurementAsync(
      MakeMeasurement(deviceId, tenant, timestamp, contentItem, request)
    );

  private SchneiderMeasurement MakeMeasurement(
    string deviceId,
    string tenant,
    DateTimeOffset timestamp,
    ContentItem _,
    SchneiderPushRequest request
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

  public SchneiderPushHandler(IOzdsTimeseriesClient measurementClient)
  {
    _measurementClient = measurementClient;
  }

  private readonly IOzdsTimeseriesClient _measurementClient;
}
