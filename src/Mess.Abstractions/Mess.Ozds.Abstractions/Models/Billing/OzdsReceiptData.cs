using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsReceiptData(
  ContentItem RegulatoryAgencyCatalogue,
  ContentItem DistributionSystemOperator,
  ContentItem ClosedDistributionSystem,
  ContentItem DistributionSystemUnit,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsReceiptExpenditureData UsageExpenditure,
  OzdsReceiptExpenditureData SupplyExpenditure,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsReceiptExpenditureData(
  decimal HighEnergyFee,
  decimal LowEnergyFee,
  decimal EnergyFee,
  decimal HighReactiveEnergyFee,
  decimal LowReactiveEnergyFee,
  decimal ReactiveEnergyFee,
  decimal MaxPowerFee,
  decimal IotDeviceFee,
  decimal RenewableEnergyFee,
  decimal BusinessUsageFee,
  decimal Total
);
