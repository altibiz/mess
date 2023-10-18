using Mess.Billing.Abstractions;
using Mess.Billing.Abstractions.Services;
using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.ViewModels;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.System.Extensions.Timestamps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Users.Services;
using YesSql;
using ContentAdminController = OrchardCore.Contents.Controllers.AdminController;
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Billing.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> Bills()
  {
    PaymentsViewModel viewModel = new();
    IEnumerable<ContentItem>? contentItems = null;

    if (
      await _authorizationService.AuthorizeAsync(
        User,
        Permissions.ListIssuedBills
      )
    )
    {
      var orchardCoreUser =
        await _userService.GetAuthenticatedOrchardCoreUserAsync(User);
      var issuerItem = await _session
        .Query<ContentItem, UserPickerFieldIndex>()
        .Where(
          index =>
            index.SelectedUserId == orchardCoreUser.UserId
            && index.ContentPart == "LegalEntityPart"
        )
        .FirstOrDefaultAsync();
      contentItems = await _session
        .Query<ContentItem, BillingIndex>()
        .Where(index => index.IssuerContentItemId == issuerItem.ContentItemId)
        .ListAsync();
    }
    else if (
      await _authorizationService.AuthorizeAsync(
        User,
        Permissions.ListReceivedBills
      )
    )
    {
      var orchardCoreUser =
        await _userService.GetAuthenticatedOrchardCoreUserAsync(User);
      var recipientItem = await _session
        .Query<ContentItem, UserPickerFieldIndex>()
        .Where(
          index =>
            index.SelectedUserId == orchardCoreUser.UserId
            && index.ContentPart == "LegalEntityPart"
        )
        .FirstOrDefaultAsync();
      contentItems = await _session
        .Query<ContentItem, BillingIndex>()
        .Where(
          index => index.RecipientContentItemId == recipientItem.ContentItemId
        )
        .ListAsync();
    }
    else
    {
      return Forbid();
    }

    var payments = contentItems
      .Where(contentItem => contentItem.Has<InvoicePart>())
      .Select(
        contentItem =>
          new PaymentViewModel
          {
            InvoiceItem = contentItem,
            ReceiptItem = contentItems
              .Where(
                contentItem =>
                  contentItem.Has<ReceiptPart>()
                  && contentItem
                    .As<ReceiptPart>()
                    .Invoice.ContentItemIds.Contains(contentItem.ContentItemId)
              )
              .FirstOrDefault()
          }
      )
      .ToList();

    viewModel.Payments = payments;

    return Ok(viewModel);
  }

  [HttpPost]
  public async Task<IActionResult> IssueInvoice(string contentItemId)
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

    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        Permissions.IssueInvoice,
        billingItem
      )
    )
    {
      return Forbid();
    }

    var billingFactory = _serviceProvider
      .GetServices<IBillingFactory>()
      .Where(factory => factory.IsApplicable(billingItem))
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
      billingItem,
      nowLastMonthStart,
      nowLastMonthEnd
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

    if (
      !await _authorizationService.AuthorizeAsync(
        User,
        Permissions.ConfirmPayment,
        invoiceItem
      )
    )
    {
      return Forbid();
    }

    var paymentIndex = await _session
      .QueryIndex<PaymentIndex>()
      .Where(index => index.InvoiceContentItemId == invoiceItem.ContentItemId)
      .FirstOrDefaultAsync();
    if (paymentIndex == null)
    {
      return NotFound();
    }

    var billingItem = await _contentManager.GetAsync(
      paymentIndex.BillingContentItemId
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
      .Where(factory => factory.IsApplicable(billingItem))
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
    await _contentManager.CreateAsync(receiptItem);

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
    ISession session,
    IAuthorizationService authorizationService,
    IUserService userService
  )
  {
    _contentManager = contentManager;
    _serviceProvider = serviceProvider;
    _session = session;
    _authorizationService = authorizationService;
    _userService = userService;
  }

  private readonly IContentManager _contentManager;
  private readonly IServiceProvider _serviceProvider;
  private readonly ISession _session;
  private readonly IAuthorizationService _authorizationService;
  private readonly IUserService _userService;
}
