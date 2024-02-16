namespace Mess.Ozds.Abstractions.Timeseries;

public record ClosedDistributionSystemDiagnostics(
  decimal Consumption_kWh,
  decimal PeakPower_kW
);
