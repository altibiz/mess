namespace Mess.Billing.Abstractions.Receipts;

public record Receipt(
  string BillableContnetItemId,
  string IssuerContentItemId,
  string RecipientContentItemId,
  string[] PartyContentItemIds,
  string? InvoiceContentItemId,
  Guid Id,
  DateTime IssuedTimestamp,
  ReceiptSection[] Sections,
  decimal Total
);

public record ReceiptSection(
  string Title,
  ReceiptElement[] Items,
  decimal Total
);

public record ReceiptElement(string Title, decimal Amount);
