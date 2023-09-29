using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Invoices;

public record Invoice(
  ContentItem[] Parties,
  ContentItem Issuer,
  InvoiceSection[] Sections
);

public record InvoiceSection(string Title, InvoiceItem[] Items, decimal Total);

public record InvoiceItem(string Title, decimal Amount);
