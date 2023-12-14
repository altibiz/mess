using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

namespace Mess.Blazor.Abstractions.Extensions;

public static class IHtmlContentExtensions
{
  public static HtmlString ToHtmlString(this IHtmlContent content)
  {
    using var writer = new StringWriter();
    content.WriteTo(writer, HtmlEncoder.Default);
    return new HtmlString(writer.ToString());
  }

  public static MarkupString ToMarkupString(this IHtmlContent content)
  {
    using var writer = new StringWriter();
    content.WriteTo(writer, HtmlEncoder.Default);
    return new MarkupString(writer.ToString());
  }
}
