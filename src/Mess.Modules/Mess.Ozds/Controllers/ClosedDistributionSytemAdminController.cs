using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Indexes;
using OrchardCore.ContentFields.Indexing.SQL;
using Mess.OrchardCore;

namespace Mess.Ozds.Controllers;

[Admin]
public class ClosedDistributionSystemAdminController : Controller
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
    if (
      !orchardCoreUser.RoleNames.Contains("Administrator")
      && !(
        orchardCoreUser.RoleNames.Contains(
          "DistributionSystemOperatorRepresentative"
        )
        && legalEntityItem is not null
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
    )
    {
      return Forbid();
    }

    return View(
      new ClosedDistributionSystemDetailViewModel { ContentItem = contentItem }
    );
  }

  public ClosedDistributionSystemAdminController(
    IAuthorizationService authorizationService,
    IContentManager contentManager,
    ISession session,
    IOzdsTimeseriesClient measurementClient
  )
  {
    _contentManager = contentManager;
    _authorizationService = authorizationService;
    _session = session;
    _measurementClient = measurementClient;
  }

  private readonly IContentManager _contentManager;
  private readonly IAuthorizationService _authorizationService;
  private readonly ISession _session;
  private readonly IOzdsTimeseriesClient _measurementClient;
}
