namespace Mess.Ozds.Abstractions.Models;

public record OzdsReceiptData(
  DateTime From,
  DateTime To,
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
