using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Indexing.SQL;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Ozds.Controllers;

public class ClosedDistributionSystemRepresentativeController
{
  private readonly IContentManager _contentManager;
  private readonly ISession _session;

  public ClosedDistributionSystemRepresentativeController(
    IContentManager contentManager,
    ISession session
  )
  {
    _contentManager = contentManager;
    _session = session;
  }

  public async Task<IActionResult> Dashboard()
  {
    var orchardCoreUser = await this.GetAuthenticatedOrchardCoreUserAsync();

    var legalEntityItem = await _session
      .Query<ContentItem, UserPickerFieldIndex>()
      .Where(index => index.ContentPart == "LegalEntityPart")
      .Where(index => index.SelectedUserId == orchardCoreUser.UserId)
      .FirstOrDefaultAsync();
  }
}
