using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using Mess.OrchardCore;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Ozds.Controllers;

[Authorize]
public class DistributionSystemOperatorController : Controller
{
  public async Task<IActionResult> List()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();

    IEnumerable<DistributionSystemOperatorItem>? operators = null;
    if (orchardCoreUser.RoleNames.Contains("Administrator"))
    {
      operators = await _session
        .Query<ContentItem, ContentItemIndex>()
        .Where(index => index.ContentType == "DistributionSystemOperator")
        .ListContentAsync<DistributionSystemOperatorItem>();
    }
    else
    {
      return Forbid();
    }

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
    if (contentItem == null)
    {
      return NotFound();
    }
    var distributionSystemOperatorContentItemId = contentItem.ContentItemId;

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
        && distributionSystemOperatorContentItemId
          == legalEntityItem.ContentItemId
      )
    )
    {
      return Forbid();
    }

    return View(
      new DistributionSystemOperatorDetailViewModel
      {
        ContentItem = contentItem
      }
    );
  }

  public DistributionSystemOperatorController(
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
