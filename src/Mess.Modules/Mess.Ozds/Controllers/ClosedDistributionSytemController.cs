using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Ozds.Controllers;

[Authorize]
public class ClosedDistributionSystemController : Controller
{
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
    {
      systems = await _session
        .Query<ContentItem, ClosedDistributionSystemIndex>()
        .ListContentAsync<ClosedDistributionSystemItem>();
    }
    else if (
      orchardCoreUser.RoleNames.Contains(
        "DistributionSystemOperatorRepresentative"
      ) && legalEntityItem is not null
    )
    {
      systems = await _session
        .Query<ContentItem, ClosedDistributionSystemIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<ClosedDistributionSystemItem>();
    }
    else
    {
      return Forbid();
    }

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
    if (contentItem == null)
    {
      return NotFound();
    }

    var index = await _session
      .QueryIndex<ClosedDistributionSystemIndex>()
      .Where(index => index.ClosedDistributionSystemContentItemId == contentItemId)
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
          "DistributionSystemOperatorRepresentative"
        )
        && legalEntityItem is not null
        && index is not null
        && index.DistributionSystemOperatorContentItemId
          == legalEntityItem.ContentItemId
      )
      && !(
        orchardCoreUser.RoleNames.Contains(
          "ClosedDistributionSystemRepresentative"
        )
        && legalEntityItem is not null
        && index is not null
        && index.ClosedDistributionSystemContentItemId
          == legalEntityItem.ContentItemId
      )
      ? Forbid()
      : View(
      new ClosedDistributionSystemDetailViewModel { ContentItem = contentItem }
    );
  }

  public ClosedDistributionSystemController(
    IContentManager contentManager,
    ISession session
  )
  {
    _contentManager = contentManager;
    _session = session;
  }

  private readonly IContentManager _contentManager;
  private readonly ISession _session;
}
