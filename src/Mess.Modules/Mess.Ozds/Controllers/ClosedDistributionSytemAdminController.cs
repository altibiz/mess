using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Controllers;

[Admin]
public class ClosedDistributionSystemAdminController : Controller
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public ClosedDistributionSystemAdminController(
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
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();

    IEnumerable<ClosedDistributionSystemItem>? systems = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
      systems = await _session
        .Query<ContentItem, ClosedDistributionSystemIndex>()
        .ListContentAsync<ClosedDistributionSystemItem>();
    else if (
      orchardCoreUser.RoleNames.Contains(
        "Distribution System Operator Representative"
      ) && legalEntityItem is not null
    )
      systems = await _session
        .Query<ContentItem, ClosedDistributionSystemIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<ClosedDistributionSystemItem>();
    else
      return Forbid();

    return View(
      new ClosedDistributionSystemListViewModel
      {
        ContentItems = systems.ToList()
      }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId)
  {
    var contentItem =
      await _contentManager.GetContentAsync<ClosedDistributionSystemItem>(
        contentItemId
      );
    if (contentItem == null) return NotFound();

    var index = await _session
      .QueryIndex<ClosedDistributionSystemIndex>()
      .Where(
        index => index.ClosedDistributionSystemContentItemId == contentItemId
      )
      .FirstOrDefaultAsync();

    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();
    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();

    return !orchardCoreUser.RoleNames.Contains("Administrator")
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Distribution System Operator Representative"
             )
             && legalEntityItem is not null
             && index.DistributionSystemOperatorContentItemId
             == legalEntityItem.ContentItemId
           )
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Closed Distribution System Representative"
             )
             && legalEntityItem is not null
             && index is not null
             && index.ClosedDistributionSystemContentItemId
             == legalEntityItem.ContentItemId
           )
      ? Forbid()
      : View(
        new ClosedDistributionSystemDetailViewModel
        { ContentItem = contentItem }
      );
  }
}
