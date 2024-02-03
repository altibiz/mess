namespace Mess.Ozds.Abstractions.Billing;

public record OzdsIotDeviceBillingData(
  decimal HighStartEnergyTotal_kWh,
  decimal HighEndEnergyTotal_kWh,
  decimal LowStartEnergyTotal_kWh,
  decimal LowEndEnergyTotal_kWh,
  decimal StartEnergyTotal_kWh,
  decimal EndEnergyTotal_kWh,
  decimal HighStartReactiveEnergyTotal_kWh,
  decimal HighEndReactiveEnergyTotal_kWh,
  decimal LowStartReactiveEnergyTotal_kWh,
  decimal LowEndReactiveEnergyTotal_kWh,
  decimal StartReactiveEnergyTotal_kWh,
  decimal EndReactiveEnergyTotal_kWh,
  decimal PeakPowerTotal_kW
);
