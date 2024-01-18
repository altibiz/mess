namespace Mess.Ozds.Abstractions.Billing;

public record OzdsIotDeviceBillingData(
  decimal StartEnergyTotal_kWh,
  decimal EndEnergyTotal_kWh,
  decimal StartHighTariffEnergy_kWh,
  decimal EndHighTariffEnergy_kWh,
  decimal StartLowTariffEnergy_kWh,
  decimal EndLowTariffEnergy_kWh,
  decimal PeakPowerTotal_kW
);
