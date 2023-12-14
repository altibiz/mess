using Microsoft.AspNetCore.Html;
using OrchardCore.DisplayManagement.Implementation;

namespace Mess.Blazor.Abstractions.Components;

public interface IShapeTemplateComponentEngine
{
  IEnumerable<Type> TemplateBaseClasses { get; }

  Task<IHtmlContent> RenderAsync(Type componentType,
    DisplayContext displayContext
  );
}
