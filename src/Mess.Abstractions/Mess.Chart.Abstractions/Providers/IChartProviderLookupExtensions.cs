using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.Abstractions.Providers;

public static class IChartProviderLookupExtensions
{
  public static IEnumerable<SelectListItem> Options(
    this IChartProviderLookup lookup
  ) => lookup.Ids.Select(id => new SelectListItem { Value = id, Text = id });
}
