using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement.Records;

namespace Mess.Ozds.Controllers;

[Admin]
public class DistributionSystemUnitController : Controller
{
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
    {
      units = await _session
        .Query<ContentItem, ContentItemIndex>()
        .Where(index => index.ContentType == "DistributionSystemUnit")
        .ListContentAsync<DistributionSystemUnitItem>();
    }
    else if (
      orchardCoreUser.RoleNames.Contains("DistributionSystemOperator")
      && legalEntityItem is not null
    )
    {
      units = await _session
        .Query<ContentItem, DistributionSystemUnitIndex>()
        .Where(
          index =>
            index.DistributionSystemOperatorContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<DistributionSystemUnitItem>();
    }
    else if (
      orchardCoreUser.RoleNames.Contains("ClosedDistributionSystem")
      && legalEntityItem is not null
    )
    {
      units = await _session
        .Query<ContentItem, DistributionSystemUnitIndex>()
        .Where(
          index =>
            index.ClosedDistributionSystemContentItemId
            == legalEntityItem.ContentItemId
        )
        .ListContentAsync<DistributionSystemUnitItem>();
    }
    else
    {
      return Forbid();
    }

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
    if (contentItem == null)
    {
      return NotFound();
    }

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
    if (
      !orchardCoreUser.RoleNames.Contains("Administrator")
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
      && !(
        orchardCoreUser.RoleNames.Contains(
          "DistributionSystemUnitRepresentative"
        )
        && legalEntityItem is not null
        && index is not null
        && index.DistributionSystemUnitContentItemId
          == legalEntityItem.ContentItemId
      )
    )
    {
      return Forbid();
    }

    return View(
      new DistributionSystemUnitDetailViewModel { ContentItem = contentItem }
    );
  }

  public DistributionSystemUnitController(
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