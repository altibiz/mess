namespace Mess.Ozds.Abstractions.Timeseries;

public record AbbAggregate(
  string Source,
  DateTimeOffset Timestamp,
  TimeSpan TimeSpan,
  long AggregateCount,
  decimal VoltageL1Avg_V,
  decimal VoltageL2Avg_V,
  decimal VoltageL3Avg_V,
  decimal CurrentL1Avg_A,
  decimal CurrentL2Avg_A,
  decimal CurrentL3Avg_A,
  decimal ActivePowerL1Avg_W,
  decimal ActivePowerL2Avg_W,
  decimal ActivePowerL3Avg_W,
  decimal ReactivePowerL1Avg_VAR,
  decimal ReactivePowerL2Avg_VAR,
  decimal ReactivePowerL3Avg_VAR,
  decimal ActiveEnergyImportTotalMin_Wh,
  decimal ActiveEnergyImportTotalMax_Wh,
  decimal ActiveEnergyImport_Wh,
  decimal ActivePowerImportAvg_W,
  decimal ActiveEnergyExportTotalMin_Wh,
  decimal ActiveEnergyExportTotalMax_Wh,
  decimal ActiveEnergyExport_Wh,
  decimal ActivePowerExportAvg_W,
  decimal ReactiveEnergyImportTotalMin_VARh,
  decimal ReactiveEnergyImportTotalMax_VARh,
  decimal ReactiveEnergyImport_VARh,
  decimal ReactivePowerImportAvg_VAR,
  decimal ReactiveEnergyExportTotalMin_VARh,
  decimal ReactiveEnergyExportTotalMax_VARh,
  decimal ReactiveEnergyExport_VARh,
  decimal ReactivePowerExportAvg_VAR
);
