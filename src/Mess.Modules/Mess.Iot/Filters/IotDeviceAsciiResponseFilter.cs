using Mess.Prelude.Extensions.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mess.Iot.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class IotDeviceAsciiResponseAttribute : ActionFilterAttribute
{
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    if (
      context.Result is ObjectResult objectResult
      && objectResult.Value is string response
    )
      objectResult.Value = response.ToAscii();
  }
}
