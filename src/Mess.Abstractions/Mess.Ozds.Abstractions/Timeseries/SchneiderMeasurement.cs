namespace Mess.Ozds.Abstractions.Timeseries;

public record SchneiderMeasurement(
  string Tenant,
  string DeviceId,
  DateTimeOffset Timestamp,
  decimal? VoltageL1_V,
  decimal? VoltageL2_V,
  decimal? VoltageL3_V,
  decimal? VoltageAvg_V,
  decimal? CurrentL1_A,
  decimal? CurrentL2_A,
  decimal? CurrentL3_A,
  decimal? CurrentAvg_A,
  decimal? ActivePowerL1_kW,
  decimal? ActivePowerL2_kW,
  decimal? ActivePowerL3_kW,
  decimal? ActivePowerTotal_kW,
  decimal? ReactivePowerTotal_kVAR,
  decimal? ApparentPowerTotal_kVA,
  decimal? PowerFactorTotal,
  decimal? ActiveEnergyImportTotal_Wh,
  decimal? ActiveEnergyExportTotal_Wh,
  decimal? ActiveEnergyImportRateA_Wh,
  decimal? ActiveEnergyImportRateB_Wh
);
