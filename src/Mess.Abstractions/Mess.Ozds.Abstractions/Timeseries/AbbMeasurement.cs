using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Abstractions.Timeseries;

public record AbbMeasurement(
  string Source,
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
  decimal ReactivePowerL1_VAR,
  decimal ReactivePowerL2_VAR,
  decimal ReactivePowerL3_VAR,
  decimal ActivePowerImportL1_Wh,
  decimal ActivePowerImportL2_Wh,
  decimal ActivePowerImportL3_Wh,
  decimal ActivePowerExportL1_Wh,
  decimal ActivePowerExportL2_Wh,
  decimal ActivePowerExportL3_Wh,
  decimal ReactivePowerImportL1_VARh,
  decimal ReactivePowerImportL2_VARh,
  decimal ReactivePowerImportL3_VARh,
  decimal ReactivePowerExportL1_VARh,
  decimal ReactivePowerExportL2_VARh,
  decimal ReactivePowerExportL3_VARh,
  decimal ActiveEnergyImportTotal_Wh,
  decimal ActiveEnergyExportTotal_Wh,
  decimal ReactiveEnergyImportTotal_VARh,
  decimal ReactiveEnergyExportTotal_VARh
);

public static class AbbMeasurementExtensions
{
  public static bool IsValid(this AbbMeasurement measurement, AbbIotDeviceItem item) =>
    item.AbbIotDevicePart.Value.MinVoltage_V.Value is { } minVoltage_V
      && measurement.VoltageL1_V >= minVoltage_V
      && measurement.VoltageL2_V >= minVoltage_V
      && measurement.VoltageL3_V >= minVoltage_V
    && item.AbbIotDevicePart.Value.MaxVoltage_V.Value is { } maxVoltage_V
      && measurement.VoltageL1_V < maxVoltage_V
      && measurement.VoltageL2_V < maxVoltage_V
      && measurement.VoltageL3_V < maxVoltage_V
    && item.AbbIotDevicePart.Value.MinCurrent_A.Value is { } minCurrent_A
      && measurement.CurrentL1_A >= minCurrent_A
      && measurement.CurrentL2_A >= minCurrent_A
      && measurement.CurrentL3_A >= minCurrent_A
    && item.AbbIotDevicePart.Value.MaxCurrent_A.Value is { } maxCurrent_A
      && measurement.CurrentL1_A < maxCurrent_A
      && measurement.CurrentL2_A < maxCurrent_A
      && measurement.CurrentL3_A < maxCurrent_A
    && item.AbbIotDevicePart.Value.MinActivePower_W.Value is { } minActivePower_W
      && measurement.ActivePowerL1_W >= minActivePower_W
      && measurement.ActivePowerL2_W >= minActivePower_W
      && measurement.ActivePowerL3_W >= minActivePower_W
    && item.AbbIotDevicePart.Value.MaxActivePower_W.Value is { } maxActivePower_W
      && measurement.ActivePowerL1_W < maxActivePower_W
      && measurement.ActivePowerL2_W < maxActivePower_W
      && measurement.ActivePowerL3_W < maxActivePower_W
    && item.AbbIotDevicePart.Value.MinReactivePower_VAR.Value is { } minReactivePower_VAR
      && measurement.ReactivePowerL1_VAR >= minReactivePower_VAR
      && measurement.ReactivePowerL2_VAR >= minReactivePower_VAR
      && measurement.ReactivePowerL3_VAR >= minReactivePower_VAR
    && item.AbbIotDevicePart.Value.MaxReactivePower_VAR.Value is { } maxReactivePower_VAR
      && measurement.ReactivePowerL1_VAR < maxReactivePower_VAR
      && measurement.ReactivePowerL2_VAR < maxReactivePower_VAR
      && measurement.ReactivePowerL3_VAR < maxReactivePower_VAR;
}
