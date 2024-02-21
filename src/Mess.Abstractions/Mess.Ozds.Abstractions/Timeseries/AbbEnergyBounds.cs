namespace Mess.Ozds.Abstractions.Timeseries;

public record AbbEnergyBounds(
  string Source,
  DateTimeOffset Timestamp,
  TimeSpan TimeSpan,
  decimal ActiveEnergyImportTotalMin_Wh,
  decimal ActiveEnergyImportTotalMax_Wh,
  decimal ActiveEnergyImport_Wh,
  decimal ActivePowerImportAverage_W,
  decimal ActiveEnergyExportTotalMin_Wh,
  decimal ActiveEnergyExportTotalMax_Wh,
  decimal ActiveEnergyExport_Wh,
  decimal ActivePowerExportAverage_W,
  decimal ReactiveEnergyImportTotalMin_VARh,
  decimal ReactiveEnergyImportTotalMax_VARh,
  decimal ReactiveEnergyImport_VARh,
  decimal ReactivePowerImportAverage_VAR,
  decimal ReactiveEnergyExportTotalMin_VARh,
  decimal ReactiveEnergyExportTotalMax_VARh,
  decimal ReactiveEnergyExport_VARh,
  decimal ReactivePowerExportAverage_VAR
);
