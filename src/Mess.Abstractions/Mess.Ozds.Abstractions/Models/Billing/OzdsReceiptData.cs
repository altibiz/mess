using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsReceiptData(
  ContentItem DistributionSystemOperator,
  ContentItem ClosedDistributionSystem,
  ContentItem DistributionSystemUnit,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsReceiptUsageExpenditureData UsageExpenditure,
  OzdsReceiptSupplyExpenditureData SupplyExpenditure,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsReceiptUsageExpenditureData(
  decimal HighEnergyFee,
  decimal LowEnergyFee,
  decimal EnergyFee,
  decimal ReactiveEnergyFee,
  decimal MaxPowerFee,
  decimal IotDeviceFee,
  decimal Total
);

public record OzdsReceiptSupplyExpenditureData(
  decimal HighEnergyFee,
  decimal LowEnergyFee,
  decimal RenewableEnergyFee,
  decimal BusinessUsageFee,
  decimal Total
);
