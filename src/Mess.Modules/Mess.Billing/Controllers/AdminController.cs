using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Controllers;

[Admin]
public class AdminController : Controller
{
  public async Task<IActionResult> ConfirmPayment(string contentItemId)
  {
    var receipt = await _contentManager.GetAsync(contentItemId);
    if (receipt == null) {
      return NotFound();
    }

    return Ok();
  }

  public AdminController(
    IContentManager contentManager
  )
  {
    _contentManager = contentManager;
  }

  private readonly IContentManager _contentManager;
}
