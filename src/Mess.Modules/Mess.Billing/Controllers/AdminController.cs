using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Receipts;
using Mess.OrchardCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> ConfirmPayment(string contentItemId)
  {
    var invoiceItem = await _contentManager.GetContentAsync<InvoiceItem>(
      contentItemId
    );
    if (invoiceItem == null)
    {
      return NotFound();
    }
    if (invoiceItem.InvoicePart.Value.Invoice.ReceiptContentItemId is not null)
    {
      return BadRequest();
    }

    var billableItem = await _contentManager.GetAsync(
      invoiceItem.InvoicePart.Value.Invoice.BillableContnetItemId
    );
    if (billableItem == null)
    {
      return NotFound();
    }

    var receiptFactory = _serviceProvider
      .GetServices<IReceiptFactory>()
      .Where(factory => factory.ContentType == billableItem.ContentType)
      .FirstOrDefault();
    if (receiptFactory == null)
    {
      throw new NotImplementedException(
        $"No receipt factory for {billableItem.ContentType}"
      );
    }

    var receipt = await receiptFactory.CreateAsync(
      contentItem: billableItem,
      invoice: invoiceItem
    );

    var receiptItem = await _contentManager.NewContentAsync<ReceiptItem>();
    receiptItem.Alter(
      receiptItem => receiptItem.ReceiptPart,
      receiptPart =>
      {
        receiptPart.Receipt = receipt;
      }
    );
    await _contentManager.CreateAsync(receiptItem);

    invoiceItem.Alter(
      invoiceItem => invoiceItem.InvoicePart,
      invoicePart =>
      {
        invoicePart.Invoice = invoicePart.Invoice with
        {
          ReceiptContentItemId = receiptItem.ContentItemId
        };
      }
    );
    await _contentManager.UpdateAsync(invoiceItem);

    return Ok();
  }

  public AdminController(
    IContentManager contentManager,
    IServiceProvider serviceProvider
  )
  {
    _contentManager = contentManager;
    _serviceProvider = serviceProvider;
  }

  private readonly IContentManager _contentManager;
  private readonly IServiceProvider _serviceProvider;
}
