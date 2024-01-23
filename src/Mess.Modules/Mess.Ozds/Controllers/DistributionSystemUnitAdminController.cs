using Mess.Cms;
using Mess.Cms.Extensions.Microsoft;
using Mess.Ozds.Abstractions.Indexes;
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
public class DistributionSystemUnitAdminController : Controller
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public DistributionSystemUnitAdminController(
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

    IEnumerable<DistributionSystemUnitItem>? units = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
      units = await _session
        .Query<ContentItem, ContentItemIndex>()
        .Where(index => index.ContentType == "DistributionSystemUnit")
        .ListContentAsync<DistributionSystemUnitItem>();
    else if (
      orchardCoreUser.RoleNames.Contains("DistributionSystemOperator")
      && legalEntityItem is not null
    )
      units = await _session
        .Query<ContentItem, DistributionSystemUnitIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<DistributionSystemUnitItem>();
    else if (
      orchardCoreUser.RoleNames.Contains("ClosedDistributionSystem")
      && legalEntityItem is not null
    )
      units = await _session
        .Query<ContentItem, DistributionSystemUnitIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<DistributionSystemUnitItem>();
    else
      return Forbid();

    return View(
      new DistributionSystemUnitListViewModel { ContentItems = units.ToList() }
    );
  }

  public async Task<IActionResult> Detail(string contentItemId)
  {
    var contentItem =
      await _contentManager.GetContentAsync<DistributionSystemUnitItem>(
        contentItemId
      );
    if (contentItem == null) return NotFound();

    var index = await _session
      .QueryIndex<DistributionSystemUnitIndex>()
      .Where(
        index => index.DistributionSystemUnitContentItemId == contentItemId
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
             && index is not null
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
           && !(
             orchardCoreUser.RoleNames.Contains(
               "Distribution System Unit Representative"
             )
             && legalEntityItem is not null
             && index is not null
             && index.DistributionSystemUnitContentItemId
             == legalEntityItem.ContentItemId
           )
      ? Forbid()
      : View(
        new DistributionSystemUnitDetailViewModel { ContentItem = contentItem }
      );
  }
}
