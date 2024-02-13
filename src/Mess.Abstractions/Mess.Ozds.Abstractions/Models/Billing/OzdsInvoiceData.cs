using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsInvoiceData(
  ContentItem DistributionSystemOperator,
  ContentItem ClosedDistributionSystem,
  ContentItem DistributionSystemUnit,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsInvoiceUsageExpenditureData UsageExpenditure,
  OzdsInvoiceSupplyExpenditureData SupplyExpenditure,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsInvoiceUsageExpenditureData(
  decimal HighEnergyFee,
  decimal LowEnergyFee,
  decimal EnergyFee,
  decimal ReactiveEnergyFee,
  decimal MaxPowerFee,
  decimal IotDeviceFee,
  decimal Total
);

public record OzdsInvoiceSupplyExpenditureData(
  decimal HighEnergyFee,
  decimal LowEnergyFee,
  decimal RenewableEnergyFee,
  decimal BusinessUsageFee,
  decimal Total
);
