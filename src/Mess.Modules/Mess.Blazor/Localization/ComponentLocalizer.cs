using System.Diagnostics;
using System.Text;
using Mess.Blazor.Abstractions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

// TODO: fix contextualization

namespace Microsoft.AspNetCore.Mvc.Localization;

/// <summary>
/// An <see cref="IViewLocalizer"/> implementation that derives the resource location from the executing view's
/// file path.
/// </summary>
public class ComponentLocalizer : IComponentLocalizer
{
  private readonly IHtmlLocalizerFactory _localizerFactory;
  private readonly string _applicationName;
  private IHtmlLocalizer _localizer = default!;

  /// <summary>
  /// Creates a new <see cref="ViewLocalizer"/>.
  /// </summary>
  /// <param name="localizerFactory">The <see cref="IHtmlLocalizerFactory"/>.</param>
  /// <param name="hostingEnvironment">The <see cref="IWebHostEnvironment"/>.</param>
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

  /// <inheritdoc />
  public virtual LocalizedHtmlString this[string key]
  {
    get
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      return _localizer[key];
    }
  }

  /// <inheritdoc />
  public virtual LocalizedHtmlString this[string key, params object[] arguments]
  {
    get
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      return _localizer[key, arguments];
    }
  }

  /// <inheritdoc />
  public LocalizedString GetString(string name) => _localizer.GetString(name);

  /// <inheritdoc />
  public LocalizedString GetString(string name, params object[] values) => _localizer.GetString(name, values);

  /// <inheritdoc />
  public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
      _localizer.GetAllStrings(includeParentCultures);

  /// <summary>
  /// Apply the specified <see cref="ViewContext"/>.
  /// </summary>
  /// <param name="viewContext">The <see cref="ViewContext"/>.</param>
  public void Contextualize(Type componentType)
  {
    if (componentType == null)
    {
      throw new ArgumentNullException(nameof(componentType));
    }

    // Given a view path "/Views/Home/Index.cshtml" we want a baseName like "MyApplication.Views.Home.Index"
    var name = $"{componentType.Namespace}.{componentType.Name}";

    Debug.Assert(!string.IsNullOrEmpty(name), "Couldn't determine a path for the view");

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
