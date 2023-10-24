using Mess.Ozds.Abstractions.Timeseries;
using Mess.Ozds.ViewModels;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using YesSql;
using Mess.Ozds.Abstractions.Models;
using OrchardCore.ContentManagement.Records;
using Mess.OrchardCore;
using OrchardCore.ContentFields.Indexing.SQL;

namespace Mess.Ozds.Controllers;

[Admin]
public class DistributionSystemOperatorAdminController : Controller
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
    var distributionSystemOperatorItem = await _session
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
        && distributionSystemOperatorItem is not null
        && distributionSystemOperatorContentItemId
          == distributionSystemOperatorItem.ContentItemId
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

  public DistributionSystemOperatorAdminController(
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
