namespace Mess.Ozds.Abstractions.Models;

public record OzdsCalculationData(
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsExpenditureData UsageExpenditure,
  OzdsExpenditureData SupplyExpenditure
);

public record OzdsExpenditureData(
  OzdsExpenditureItemData? HighEnergyItem,
  OzdsExpenditureItemData? LowEnergyItem,
  OzdsExpenditureItemData? EnergyItem,
  OzdsExpenditureItemData? MaxPowerItem,
  OzdsExpenditureItemData? MeasurementDeviceFee,
  decimal Total
);

public record OzdsExpenditureItemData(
  decimal ValueFrom,
  decimal ValueTo,
  decimal Amount,
  decimal UnitPrice,
  decimal Total
);
