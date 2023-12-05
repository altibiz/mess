using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;

namespace Mess.Ozds.Controllers;

[Authorize]
public class DistributionSystemOperatorController : Controller
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public DistributionSystemOperatorController(
    IContentManager contentManager,
    ISession session
  )
  {
    _contentManager = contentManager;
    _session = session;
  }

  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();

    IEnumerable<DistributionSystemOperatorItem>? operators = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
      operators = await _session
        .Query<ContentItem, ContentItemIndex>()
        .Where(index => index.ContentType == "DistributionSystemOperator")
        .ListContentAsync<DistributionSystemOperatorItem>();
    else
      return Forbid();

    return View(
      new DistributionSystemOperatorListViewModel
      {
        ContentItems = operators.ToList()
      }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId)
  {
    var contentItem =
      await _contentManager.GetContentAsync<DistributionSystemOperatorItem>(
        contentItemId
      );
    if (contentItem == null) return NotFound();
    var distributionSystemOperatorContentItemId = contentItem.ContentItemId;

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();

    return !orchardCoreUser.RoleNames.Contains("Administrator")
           && !(
             orchardCoreUser.RoleNames.Contains(
               "DistributionSystemOperatorRepresentative"
             )
             && legalEntityItem is not null
             && distributionSystemOperatorContentItemId
             == legalEntityItem.ContentItemId
           )
      ? Forbid()
      : View(
        new DistributionSystemOperatorDetailViewModel
        {
          ContentItem = contentItem
        }
      );
  }
}
