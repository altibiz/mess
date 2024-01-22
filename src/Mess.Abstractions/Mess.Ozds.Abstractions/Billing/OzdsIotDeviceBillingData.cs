namespace Mess.Ozds.Abstractions.Billing;

public record OzdsIotDeviceBillingData(
  decimal StartEnergyTotal_kWh,
  decimal EndEnergyTotal_kWh,
  decimal PeakPowerTotal_kW
);
