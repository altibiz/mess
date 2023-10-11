using Mess.Billing.Abstractions.Factory;
using Mess.Billing.Abstractions.Models;
using Mess.System.Extensions.Timestamps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Mvc.Core.Utilities;
using YesSql;
using ContentAdminController = OrchardCore.Contents.Controllers.AdminController;

namespace Mess.Billing.Controllers;

[Admin]
public class AdminController : Controller
{
  [HttpGet]
  public async Task<IActionResult> ListPayments()
  {
    return Ok();
  }

  [HttpGet]
  public async Task<IActionResult> ListOwnPayments()
  {
    return Ok();
  }

  [HttpPost]
  public async Task<IActionResult> CreateInvoice(string contentItemId)
  {
    var billingItem = await _contentManager.GetAsync(contentItemId);
    if (billingItem == null)
    {
      return NotFound();
    }
    var billingPart = billingItem.As<BillingPart>();
    if (billingPart == null)
    {
      return BadRequest();
    }

    var billingFactory = _serviceProvider
      .GetServices<IBillingFactory>()
      .Where(factory => factory.ContentType == billingItem.ContentType)
      .FirstOrDefault();
    if (billingFactory == null)
    {
      throw new NotImplementedException(
        $"No receipt factory for {billingItem.ContentType}"
      );
    }

    (DateTimeOffset nowLastMonthStart, DateTimeOffset nowLastMonthEnd) =
      DateTimeOffset.UtcNow.GetMonthRange();
    var invoiceItem = await billingFactory.CreateInvoiceAsync(
      from: nowLastMonthStart,
      to: nowLastMonthEnd,
      contentItem: billingItem
    );
    invoiceItem.Alter<InvoicePart>(invoicePart =>
    {
      invoicePart.BillingContentItemId = billingItem.ContentItemId;
      invoicePart.LegalEntityContentItemId =
        billingPart.LegalEntityContentItemId;
    });
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
    var invoiceItem = await _contentManager.GetAsync(contentItemId);
    if (invoiceItem == null)
    {
      return NotFound();
    }

    var invoicePart = invoiceItem.As<InvoicePart>();
    if (invoicePart == null)
    {
      return BadRequest();
    }

    var billingItem = await _contentManager.GetAsync(
      invoicePart.BillingContentItemId
    );
    if (billingItem == null)
    {
      return NotFound();
    }
    var billingPart = billingItem.As<BillingPart>();
    if (billingPart == null)
    {
      return BadRequest();
    }

    var receiptFactory = _serviceProvider
      .GetServices<IBillingFactory>()
      .Where(factory => factory.ContentType == billingItem.ContentType)
      .FirstOrDefault();
    if (receiptFactory == null)
    {
      throw new NotImplementedException(
        $"No receipt factory for {billingItem.ContentType}"
      );
    }

    var receiptItem = await receiptFactory.CreateReceiptAsync(
      contentItem: billingItem,
      invoiceContentItem: invoiceItem
    );
    receiptItem.Alter<ReceiptPart>(receiptPart =>
    {
      receiptPart.BillingContentItemId = billingItem.ContentItemId;
      receiptPart.LegalEntityContentItemId =
        billingPart.LegalEntityContentItemId;
      receiptPart.InvoiceContentItemId = invoiceItem.ContentItemId;
    });
    await _contentManager.CreateAsync(receiptItem);

    invoiceItem.Alter<InvoicePart>(invoicePart =>
    {
      invoicePart.ReceiptContentItemId = receiptItem.ContentItemId;
    });
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
