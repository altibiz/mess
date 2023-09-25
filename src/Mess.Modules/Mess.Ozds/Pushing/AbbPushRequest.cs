namespace Mess.Ozds.Pushing;

public record AbbPushRequest(
  float? CurrentL1,
  float? CurrentL2,
  float? CurrentL3,
  float? VoltageL1,
  float? VoltageL2,
  float? VoltageL3,
  float? ActivePowerL1,
  float? ActivePowerL2,
  float? ActivePowerL3,
  float? ReactivePowerL1,
  float? ReactivePowerL2,
  float? ReactivePowerL3,
  float? ApparentPowerL1,
  float? ApparentPowerL2,
  float? ApparentPowerL3,
  float? PowerFactorL1,
  float? PowerFactorL2,
  float? PowerFactorL3
);
