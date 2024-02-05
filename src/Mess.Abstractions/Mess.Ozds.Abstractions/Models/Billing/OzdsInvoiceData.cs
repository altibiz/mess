using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsInvoiceData(
  ContentItem RegulatoryAgencyCatalogue,
  ContentItem DistributionSystemOperator,
  ContentItem ClosedDistributionSystem,
  ContentItem DistributionSystemUnit,
  DateTimeOffset From,
  DateTimeOffset To,
  OzdsInvoiceExpenditureData UsageExpenditure,
  OzdsInvoiceExpenditureData SupplyExpenditure,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsInvoiceExpenditureData(
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
