namespace Mess.Ozds.Abstractions.Billing;

public record OzdsIotDeviceBillingData(
  string Source,
  DateTimeOffset FromDate,
  DateTimeOffset ToDate,
  decimal HighTariffMinEnergyImportTotal_kWh,
  decimal HighTariffMaxEnergyImportTotal_kWh,
  decimal LowTariffMinEnergyImportTotal_kWh,
  decimal LowTariffMaxEnergyImportTotal_kWh,
  decimal MinEnergyImportTotal_kWh,
  decimal MaxEnergyImportTotal_kWh,
  decimal MinReactiveEnergyImportTotal_kVARh,
  decimal MaxReactiveEnergyImportTotal_kVARh,
  decimal MinReactiveEnergyExportTotal_kVARh,
  decimal MaxReactiveEnergyExportTotal_kVARh,
  decimal PeakPower_kW
);
