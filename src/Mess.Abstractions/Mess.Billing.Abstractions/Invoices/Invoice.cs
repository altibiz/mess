namespace Mess.Billing.Abstractions.Invoices;

public record Invoice(
  string BillableContnetItemId,
  string IssuerContentItemId,
  string RecipientContentItemId,
  string[] PartyContentItemIds,
  string? InvoiceContentItemId,
  Guid Id,
  DateTime IssuedTimestamp,
  InvoiceSection[] Sections,
  decimal Total
);

public record InvoiceSection(string Title, InvoiceElement[] Items, decimal Total);

public record InvoiceElement(string Title, decimal Amount);
