using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Mvc.Localization;

namespace Mess.Blazor.Abstractions.Localization;

public interface IComponentLocalizer : IHtmlLocalizer, IComponentAware
{ }
