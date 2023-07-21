using Microsoft.AspNetCore.Mvc.Filters;
using OrchardCore.ContentManagement;

namespace Mess.MeasurementDevice.Abstractions.Security;

public interface IMeasurementDeviceAuthorizationHandler
{
  public string ContentType { get; }

  public Task AuthorizeAsync(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  );

  public void Authorize(
    AuthorizationFilterContext context,
    ContentItem measurementDevice
  );
}
