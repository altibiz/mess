using OrchardCore.ContentManagement;

namespace Mess.Ozds.Abstractions.Models;

public record OzdsReceiptData(
  ContentItem RegulatoryAgencyCatalogue,
  ContentItem DistributionSystemOperator,
  ContentItem ClosedDistributionSystem,
  ContentItem DistributionSystemUnit,
  DateTimeOffset From,
  DateTimeOffset To,
  decimal UsageFee,
  decimal SupplyFee,
  OzdsReceiptFeeData? RenewableEnergyFee,
  OzdsReceiptFeeData? BusinessUsageFee,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsReceiptFeeData(
  decimal Amount,
  decimal UnitPrice,
  decimal Total
);
