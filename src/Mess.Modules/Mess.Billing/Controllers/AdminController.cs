using Mess.Billing.Abstractions.Invoices;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Receipts;
using Mess.OrchardCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Mvc.Core.Utilities;
using YesSql;
using ContentAdminController = global::OrchardCore.Contents.Controllers.AdminController;

namespace Mess.Billing.Controllers;

[Admin]
public class AdminController : Controller
{
  [HttpPost]
  public async Task<IActionResult> CreateInvoice(string contentItemId)
  {
    var billingContentItem = await _contentManager.GetAsync(contentItemId);
    if (billingContentItem == null)
    {
      return NotFound();
    }

    var billingPart = billingContentItem.As<BillingPart>();
    if (billingPart == null)
    {
      return BadRequest();
    }

    var invoiceFactory = _serviceProvider
      .GetServices<IInvoiceFactory>()
      .Where(factory => factory.ContentType == billingContentItem.ContentType)
      .FirstOrDefault();
    if (invoiceFactory == null)
    {
      throw new NotImplementedException(
        $"No receipt factory for {billingContentItem.ContentType}"
      );
    }

    var catalogueContentItems = (
      await _contentManager.GetAsync(billingPart.CatalogueContentItemIds)
    ).ToArray();

    var invoice = await invoiceFactory.CreateAsync(
      contentItem: billingContentItem,
      catalogueContentItems: catalogueContentItems
    );
    var invoiceItem = await _contentManager.NewContentAsync<InvoiceItem>();
    invoiceItem.Alter(
      invoiceItem => invoiceItem.InvoicePart,
      invoicePart =>
      {
        invoicePart.Invoice = invoice;
      }
    );
    await _contentManager.CreateAsync(invoiceItem);

    return RedirectToAction(
      actionName: nameof(ContentAdminController.Display),
      controllerName: typeof(ContentAdminController).ControllerName(),
      routeValues: new
      {
        area = "OrchardCore.Contents",
        contentItemId = invoiceItem.ContentItemId
      }
    );
  }

  [HttpPost]
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

    var billingItem = await _contentManager.GetAsync(
      invoiceItem.InvoicePart.Value.Invoice.BillableContnetItemId
    );
    if (billingItem == null)
    {
      return NotFound();
    }

    var receiptFactory = _serviceProvider
      .GetServices<IReceiptFactory>()
      .Where(factory => factory.ContentType == billingItem.ContentType)
      .FirstOrDefault();
    if (receiptFactory == null)
    {
      throw new NotImplementedException(
        $"No receipt factory for {billingItem.ContentType}"
      );
    }

    var receipt = await receiptFactory.CreateAsync(
      contentItem: billingItem,
      invoiceContentItem: invoiceItem
    );
    _session.Save(receipt);

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

    return RedirectToAction(
      actionName: nameof(ContentAdminController.Display),
      controllerName: typeof(ContentAdminController).ControllerName(),
      routeValues: new
      {
        area = "OrchardCore.Contents",
        contentItemId = invoiceItem.ContentItemId
      }
    );
  }

  public AdminController(
    IContentManager contentManager,
    IServiceProvider serviceProvider,
    ISession session
  )
  {
    _contentManager = contentManager;
    _serviceProvider = serviceProvider;
    _session = session;
  }

  private readonly IContentManager _contentManager;
  private readonly IServiceProvider _serviceProvider;
  private readonly ISession _session;
}