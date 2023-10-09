namespace Mess.Ozds.Abstractions.Models;

public record OzdsInvoiceData(
  DateTimeOffset From,
  DateTimeOffset To,
  decimal UsageFee,
  decimal SupplyFee,
  OzdsInvoiceFeeData? RenewableEnergyFee,
  OzdsInvoiceFeeData? BusinessUsageFee,
  decimal Total,
  decimal TaxRate,
  decimal Tax,
  decimal TotalWithTax
);

public record OzdsInvoiceFeeData(
  decimal Amount,
  decimal UnitPrice,
  decimal Total
);
