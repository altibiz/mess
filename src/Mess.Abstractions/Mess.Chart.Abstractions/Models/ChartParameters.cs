namespace Mess.Chart.Abstractions.Models;

public record class ChartParameters(
  string Provider,
  ChartProviderParameters ProviderParameters
);

// TODO: better type for ProviderSpecific parameters?
public record class ChartProviderParameters(string ProviderSpecific);
