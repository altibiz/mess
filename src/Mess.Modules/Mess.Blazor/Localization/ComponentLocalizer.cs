using System.Diagnostics;
using System.Text;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Abstractions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace Microsoft.AspNetCore.Mvc.Localization;

public class ComponentLocalizer : IComponentLocalizer
{
  private readonly IHtmlLocalizerFactory _localizerFactory;
  private readonly string _applicationName;
  private IHtmlLocalizer _localizer = default!;

  public ComponentLocalizer(IHtmlLocalizerFactory localizerFactory, IWebHostEnvironment hostingEnvironment)
  {
    if (hostingEnvironment == null)
    {
      throw new ArgumentNullException(nameof(hostingEnvironment));
    }

    if (string.IsNullOrEmpty(hostingEnvironment.ApplicationName))
    {
      throw new InvalidOperationException($"{nameof(hostingEnvironment)}.ApplicationName must have a value.");
    }

    _applicationName = hostingEnvironment.ApplicationName;
    _localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));
  }

  public virtual MarkupString this[string key]
  {
    get
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      return _localizer[key].ToMarkupString();
    }
  }

  public virtual MarkupString this[string key, params object[] arguments]
  {
    get
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      return _localizer[key, arguments].ToMarkupString();
    }
  }

  public MarkupString GetString(string name) => new(_localizer.GetString(name));

  public MarkupString GetString(string name, params object[] values) => new(_localizer.GetString(name, values));

  public IEnumerable<MarkupString> GetAllStrings(bool includeParentCultures) =>
      _localizer
        .GetAllStrings(includeParentCultures)
        .Select(@string => new MarkupString(@string));

  public void Contextualize(Type componentType)
  {
    if (componentType == null)
    {
      throw new ArgumentNullException(nameof(componentType));
    }

    var name = $"{componentType.Namespace}.{componentType.Name}";

    _localizer = _localizerFactory.Create(BuildBaseName(name), _applicationName);
  }

  private string BuildBaseName(string path)
  {
    var extension = Path.GetExtension(path);
    var startIndex = path[0] == '/' || path[0] == '\\' ? 1 : 0;
    var length = path.Length - startIndex - extension.Length;
    var capacity = length + _applicationName.Length + 1;
    var builder = new StringBuilder(path, startIndex, length, capacity);

    builder.Replace('/', '.').Replace('\\', '.');

    // Prepend the application name
    builder.Insert(0, '.');
    builder.Insert(0, _applicationName);

    return builder.ToString();
  }
}
