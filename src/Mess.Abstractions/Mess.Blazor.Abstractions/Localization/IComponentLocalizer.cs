using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Mess.Blazor.Abstractions.Localization;

public interface IComponentLocalizer : IComponentAware
{
  //
  // Summary:
  //     Gets the string resource with the given name.
  //
  // Parameters:
  //   name:
  //     The name of the string resource.
  //
  // Returns:
  //     The string resource as a Microsoft.AspNetCore.Mvc.Localization.LocalizedHtmlString.
  MarkupString this[string name] { get; }

  //
  // Summary:
  //     Gets the string resource with the given name and formatted with the supplied
  //     arguments. The arguments will be HTML encoded.
  //
  // Parameters:
  //   name:
  //     The name of the string resource.
  //
  //   arguments:
  //     The values to format the string with.
  //
  // Returns:
  //     The formatted string resource as a Microsoft.AspNetCore.Mvc.Localization.LocalizedHtmlString.
  MarkupString this[string name, params object[] arguments] { get; }

  //
  // Summary:
  //     Gets the string resource with the given name.
  //
  // Parameters:
  //   name:
  //     The name of the string resource.
  //
  // Returns:
  //     The string resource as a Microsoft.Extensions.Localization.LocalizedString.
  MarkupString GetString(string name);

  //
  // Summary:
  //     Gets the string resource with the given name and formatted with the supplied
  //     arguments.
  //
  // Parameters:
  //   name:
  //     The name of the string resource.
  //
  //   arguments:
  //     The values to format the string with.
  //
  // Returns:
  //     The formatted string resource as a Microsoft.Extensions.Localization.LocalizedString.
  MarkupString GetString(string name, params object[] arguments);

  //
  // Summary:
  //     Gets all string resources.
  //
  // Parameters:
  //   includeParentCultures:
  //     A System.Boolean indicating whether to include strings from parent cultures.
  //
  // Returns:
  //     The strings.
  IEnumerable<MarkupString> GetAllStrings(bool includeParentCultures);
}
