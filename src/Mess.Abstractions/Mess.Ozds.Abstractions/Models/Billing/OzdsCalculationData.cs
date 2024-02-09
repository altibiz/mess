using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsCalculationData(
  ContentItem IotDevice,
  ContentItem UsageCatalogue,
  ContentItem SupplyCatalogue,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsUsageExpenditureData UsageExpenditure,
  OzdsSupplyExpenditureData SupplyExpenditure,
  decimal Total
);

public record OzdsUsageExpenditureData(
  OzdsExpenditureItemData? HighEnergyItem,
  OzdsExpenditureItemData? LowEnergyItem,
  OzdsExpenditureItemData? EnergyItem,
  OzdsExpenditureItemData? ReactiveEnergyItem,
  OzdsExpenditureItemData? MaxPowerItem,
  OzdsExpenditureItemData? IotDeviceFee,
  decimal Total
);

public record OzdsSupplyExpenditureData(
  OzdsExpenditureItemData? HighEnergyItem,
  OzdsExpenditureItemData? LowEnergyItem,
  OzdsExpenditureItemData? RenewableEnergyFee,
  OzdsExpenditureItemData? BusinessUsageFee,
  decimal Total
);

public record OzdsExpenditureItemData(
  decimal? ValueFrom,
  decimal? ValueTo,
  decimal Amount,
  decimal UnitPrice,
  decimal Total
);
