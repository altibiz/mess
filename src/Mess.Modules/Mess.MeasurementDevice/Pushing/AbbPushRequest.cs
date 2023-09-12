namespace Mess.MeasurementDevice.Pushing;

public record AbbPushRequest(
  float? currentL1,
  float? currentL2,
  float? currentL3,
  float? voltageL1,
  float? voltageL2,
  float? voltageL3,
  float? activePowerL1,
  float? activePowerL2,
  float? activePowerL3,
  float? reactivePowerL1,
  float? reactivePowerL2,
  float? reactivePowerL3,
  float? apparentPowerL1,
  float? apparentPowerL2,
  float? apparentPowerL3,
  float? powerFactorL1,
  float? powerFactorL2,
  float? powerFactorL3
);
