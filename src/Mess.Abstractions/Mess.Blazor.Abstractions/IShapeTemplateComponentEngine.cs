using Microsoft.AspNetCore.Html;
using OrchardCore.DisplayManagement.Implementation;

namespace Mess.Blazor.Abstractions;

public interface IShapeTemplateComponentEngine
{
  IEnumerable<Type> TemplateBaseClasses { get; }

  Task<IHtmlContent> RenderAsync(Type componentType,
    DisplayContext displayContext);
}
