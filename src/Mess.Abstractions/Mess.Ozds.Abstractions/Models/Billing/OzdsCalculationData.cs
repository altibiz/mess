using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsCalculationData(
  ContentItem IotDevice,
  ContentItem RegulatoryAgencyCatalogue,
  ContentItem UsageCatalogue,
  ContentItem SupplyCatalogue,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsExpenditureData UsageExpenditure,
  OzdsExpenditureData SupplyExpenditure
);

public record OzdsExpenditureData(
  OzdsExpenditureItemData? HighEnergyItem,
  OzdsExpenditureItemData? LowEnergyItem,
  OzdsExpenditureItemData? EnergyItem,
  OzdsExpenditureItemData? HighReactiveEnergyItem,
  OzdsExpenditureItemData? LowReactiveEnergyItem,
  OzdsExpenditureItemData? ReactiveEnergyItem,
  OzdsExpenditureItemData? MaxPowerItem,
  OzdsExpenditureItemData? IotDeviceFee,
  OzdsExpenditureItemData? RenewableEnergyFee,
  OzdsExpenditureItemData? BusinessUsageFee,
  decimal Total
);

public record OzdsExpenditureItemData(
  decimal ValueFrom,
  decimal ValueTo,
  decimal Amount,
  decimal UnitPrice,
  decimal Total
);
