using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Receipts;

public record Receipt(
  ContentItem[] Parties,
  ContentItem Issuer,
  ReceiptSection[] Sections
);

public record ReceiptSection(string Title, ReceiptItem[] Items, decimal Total);

public record ReceiptItem(string Title, decimal Amount);
