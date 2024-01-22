namespace Mess.Ozds.Abstractions.Timeseries;

public record SchneiderMeasurement(
  string Tenant,
  string DeviceId,
  DateTimeOffset Timestamp,
  decimal VoltageL1_V,
  decimal VoltageL2_V,
  decimal VoltageL3_V,
  decimal CurrentL1_A,
  decimal CurrentL2_A,
  decimal CurrentL3_A,
  decimal ActivePowerL1_W,
  decimal ActivePowerL2_W,
  decimal ActivePowerL3_W,
  decimal ReactivePowerTotal_VAR,
  decimal ApparentPowerTotal_VA,
  decimal ActiveEnergyImportL1_Wh,
  decimal ActiveEnergyImportL2_Wh,
  decimal ActiveEnergyImportL3_Wh,
  decimal ActiveEnergyImportTotal_Wh,
  decimal ActiveEnergyExportTotal_Wh,
  decimal ReactiveEnergyImportTotal_VARh,
  decimal ReactiveEnergyExportTotal_VARh
);
