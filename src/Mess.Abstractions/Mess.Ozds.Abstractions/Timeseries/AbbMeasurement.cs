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
  decimal ReactiveEnergyExportTotal_VARh,
  decimal ActiveEnergyImportTotalT1_Wh,
  decimal ActiveEnergyImportTotalT2_Wh
);

public static class AbbMeasurementExtensions
{
  public static bool IsValid(this AbbMeasurement measurement, AbbIotDeviceItem item)
  {
    var checkL1 = item.AbbIotDevicePart.Value.Phases.SelectedValues.Contains("L1");
    var checkL2 = item.AbbIotDevicePart.Value.Phases.SelectedValues.Contains("L2");
    var checkL3 = item.AbbIotDevicePart.Value.Phases.SelectedValues.Contains("L3");

    return item.AbbIotDevicePart.Value.MinVoltage.Value is { } minVoltage_V
      && (!checkL1 || measurement.VoltageL1_V >= minVoltage_V)
      && (!checkL2 || measurement.VoltageL2_V >= minVoltage_V)
      && (!checkL3 || measurement.VoltageL3_V >= minVoltage_V)
    && item.AbbIotDevicePart.Value.MaxVoltage.Value is { } maxVoltage_V
      && (!checkL1 || measurement.VoltageL1_V < maxVoltage_V)
      && (!checkL2 || measurement.VoltageL2_V < maxVoltage_V)
      && (!checkL3 || measurement.VoltageL3_V < maxVoltage_V)
    && item.AbbIotDevicePart.Value.MinCurrent.Value is { } minCurrent_A
      && (!checkL1 || measurement.CurrentL1_A >= minCurrent_A)
      && (!checkL2 || measurement.CurrentL2_A >= minCurrent_A)
      && (!checkL3 || measurement.CurrentL3_A >= minCurrent_A)
    && item.AbbIotDevicePart.Value.MaxCurrent.Value is { } maxCurrent_A
      && (!checkL1 || measurement.CurrentL1_A < maxCurrent_A)
      && (!checkL2 || measurement.CurrentL2_A < maxCurrent_A)
      && (!checkL3 || measurement.CurrentL3_A < maxCurrent_A)
    && item.AbbIotDevicePart.Value.MinActivePower.Value is { } minActivePower_W
      && (!checkL1 || measurement.ActivePowerL1_W >= minActivePower_W)
      && (!checkL2 || measurement.ActivePowerL2_W >= minActivePower_W)
      && (!checkL3 || measurement.ActivePowerL3_W >= minActivePower_W)
    && item.AbbIotDevicePart.Value.MaxActivePower.Value is { } maxActivePower_W
      && (!checkL1 || measurement.ActivePowerL1_W < maxActivePower_W)
      && (!checkL2 || measurement.ActivePowerL2_W < maxActivePower_W)
      && (!checkL3 || measurement.ActivePowerL3_W < maxActivePower_W)
    && item.AbbIotDevicePart.Value.MinReactivePower.Value is { } minReactivePower_VAR
      && (!checkL1 || measurement.ReactivePowerL1_VAR >= minReactivePower_VAR)
      && (!checkL2 || measurement.ReactivePowerL2_VAR >= minReactivePower_VAR)
      && (!checkL3 || measurement.ReactivePowerL3_VAR >= minReactivePower_VAR)
    && item.AbbIotDevicePart.Value.MaxReactivePower.Value is { } maxReactivePower_VAR
      && (!checkL1 || measurement.ReactivePowerL1_VAR < maxReactivePower_VAR)
      && (!checkL2 || measurement.ReactivePowerL2_VAR < maxReactivePower_VAR)
      && (!checkL3 || measurement.ReactivePowerL3_VAR < maxReactivePower_VAR);
  }
}
