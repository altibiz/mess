using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.Abstractions.Timeseries;

public record SchneiderMeasurement(
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

public static class SchneiderMeasurementExtensions
{
  public static bool IsValid(this SchneiderMeasurement measurement, SchneiderIotDeviceItem item)
  {
    var checkL1 = item.SchneiderIotDevicePart.Value.Phases.SelectedValues.Contains("L1");
    var checkL2 = item.SchneiderIotDevicePart.Value.Phases.SelectedValues.Contains("L2");
    var checkL3 = item.SchneiderIotDevicePart.Value.Phases.SelectedValues.Contains("L3");

    return item.SchneiderIotDevicePart.Value.MinVoltage.Value is { } minVoltage_V
      && (!checkL1 || measurement.VoltageL1_V >= minVoltage_V)
      && (!checkL2 || measurement.VoltageL2_V >= minVoltage_V)
      && (!checkL3 || measurement.VoltageL3_V >= minVoltage_V)
    && item.SchneiderIotDevicePart.Value.MaxVoltage.Value is { } maxVoltage_V
      && (!checkL1 || measurement.VoltageL1_V < maxVoltage_V)
      && (!checkL2 || measurement.VoltageL2_V < maxVoltage_V)
      && (!checkL3 || measurement.VoltageL3_V < maxVoltage_V)
    && item.SchneiderIotDevicePart.Value.MinCurrent.Value is { } minCurrent_A
      && (!checkL1 || measurement.CurrentL1_A >= minCurrent_A)
      && (!checkL2 || measurement.CurrentL2_A >= minCurrent_A)
      && (!checkL3 || measurement.CurrentL3_A >= minCurrent_A)
    && item.SchneiderIotDevicePart.Value.MaxCurrent.Value is { } maxCurrent_A
      && (!checkL1 || measurement.CurrentL1_A < maxCurrent_A)
      && (!checkL2 || measurement.CurrentL2_A < maxCurrent_A)
      && (!checkL3 || measurement.CurrentL3_A < maxCurrent_A)
    && item.SchneiderIotDevicePart.Value.MinActivePower.Value is { } minActivePower_W
      && (!checkL1 || measurement.ActivePowerL1_W >= minActivePower_W)
      && (!checkL2 || measurement.ActivePowerL2_W >= minActivePower_W)
      && (!checkL3 || measurement.ActivePowerL3_W >= minActivePower_W)
    && item.SchneiderIotDevicePart.Value.MaxActivePower.Value is { } maxActivePower_W
      && (!checkL1 || measurement.ActivePowerL1_W < maxActivePower_W)
      && (!checkL2 || measurement.ActivePowerL2_W < maxActivePower_W)
      && (!checkL3 || measurement.ActivePowerL3_W < maxActivePower_W)
    && item.SchneiderIotDevicePart.Value.MinReactivePower.Value is { } minReactivePower_VAR
      && measurement.ReactivePowerTotal_VAR >= 3 * minReactivePower_VAR
    && item.SchneiderIotDevicePart.Value.MaxReactivePower.Value is { } maxReactivePower_VAR
      && measurement.ReactivePowerTotal_VAR < 3 * maxReactivePower_VAR
    && item.SchneiderIotDevicePart.Value.MinApparentPower.Value is { } minApparentPower_VA
      && measurement.ApparentPowerTotal_VA >= 3 * minApparentPower_VA
    && item.SchneiderIotDevicePart.Value.MaxApparentPower.Value is { } maxApparentPower_VA
      && measurement.ApparentPowerTotal_VA < 3 * maxApparentPower_VA;
  }
}
