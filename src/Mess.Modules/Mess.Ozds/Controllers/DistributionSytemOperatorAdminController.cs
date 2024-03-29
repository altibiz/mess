using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using YesSql;

namespace Mess.Ozds.Controllers;

[Admin]
public class DistributionSystemOperatorAdminController : Controller
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public DistributionSystemOperatorAdminController(
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
        .LatestPublished()
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
    var distributionSystemOperatorItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .LatestPublished()
      .FirstOrDefaultAsync();

    return !orchardCoreUser.RoleNames.Contains("Administrator")
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Distribution System Operator Representative"
             )
             && distributionSystemOperatorItem is not null
             && distributionSystemOperatorContentItemId
             == distributionSystemOperatorItem.ContentItemId
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
