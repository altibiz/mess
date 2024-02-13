namespace Mess.Ozds.Abstractions.Billing;

public record OzdsIotDeviceBillingData(
  string Source,
  decimal HighStartEnergyImportTotal_kWh,
  decimal HighEndEnergyImportTotal_kWh,
  decimal LowStartEnergyImportTotal_kWh,
  decimal LowEndEnergyImportTotal_kWh,
  decimal StartEnergyImportTotal_kWh,
  decimal EndEnergyImportTotal_kWh,
  decimal StartReactiveEnergyImportTotal_kVARh,
  decimal EndReactiveEnergyImportTotal_kVARh,
  decimal StartReactiveEnergyExportTotal_kVARh,
  decimal EndReactiveEnergyExportTotal_kVARh,
  decimal PeakPower_kW
);
