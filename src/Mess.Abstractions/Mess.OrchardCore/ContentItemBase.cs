using System.Reflection;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardContentItem = OrchardCore.ContentManagement.ContentItem;
using Mess.System.Extensions.Objects;
using Mess.System.Extensions.Functions;
using Mess.System.Extensions.Strings;
using Mess.System.Extensions.Enumerables;
using System.Linq.Expressions;

namespace Mess.OrchardCore;

public abstract class ContentItemBase
{
  protected ContentItemBase(OrchardContentItem contentItem)
  {
    Inner = contentItem;
  }

  public OrchardContentItem Inner { get; init; }

  public Lazy<ContainedPart?> ContainedPart { get; init; } = default!;

  public static implicit operator OrchardContentItem(ContentItemBase @this) =>
    @this.Inner;

  public string ContentItemId => Inner.ContentItemId;
}

// NOTE: don't use for now - needs more testing
public abstract class ContentItemBase<TDerived>
  : ContentItemBase,
    IContentTypeBaseDerivedIndicator where TDerived : ContentItemBase<TDerived>
{
  protected ContentItemBase(OrchardContentItem item) : base(item)
  {
    (this as TDerived)!.PopulateContent();
  }
}

public static class ContentItemExtensions
{
  public static string ContentTypeName(this Type @this) =>
    @this.Name.RegexRemove("Item$");

  public static string ContentTypeName<T>() => typeof(T).ContentTypeName();

  public static T? AsContent<T>(this OrchardContentItem @this)
    where T : ContentItemBase
  {
    var contentTypeName = ContentTypeName<T>();
    if (@this.ContentType != contentTypeName)
    {
      return null;
    }

    var contentItem = (T?)
      Activator.CreateInstance(
        typeof(T),
        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
        null,
        new[] { @this },
        null
      );
    if (
      contentItem is null
      || contentItem
        .GetType()
        .IsAssignableTo(typeof(IContentTypeBaseDerivedIndicator))
    )
    {
      return null;
    }

    return contentItem.PopulateContent();
  }

  public static async Task<TItem> AlterAsync<TItem, TPart>(
    this TItem @this,
    Expression<Func<TItem, Lazy<TPart>>> expression,
    Func<TPart, Task> action
  )
    where TItem : ContentItemBase
    where TPart : ContentPart, new()
  {
    var part = @this.Inner.GetOrCreate<TPart>();
    await action(part);
    @this.Inner.Apply(part);
    return @this;
  }

  public static TItem Alter<TItem, TPart>(
    this TItem @this,
    Expression<Func<TItem, Lazy<TPart>>> expression,
    Action<TPart> action
  )
    where TItem : ContentItemBase
    where TPart : ContentPart, new()
  {
    var part = @this.Inner.GetOrCreate<TPart>();
    action(part);
    @this.Inner.Apply(part);
    return @this;
  }

  public static async Task<T?> NewContentAsync<T>(this IContentManager content)
    where T : ContentItemBase
  {
    var orchardContentItem = await content.NewAsync(
      typeof(T).Name.RegexRemove(@"Type$")
    );
    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> CloneContentAsync<T>(
    this IContentManager content,
    OrchardContentItem item
  ) where T : ContentItemBase
  {
    var orchardContentItem = await content.CloneAsync(item);
    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> GetContentAsync<T>(
    this IContentManager content,
    string id
  ) where T : ContentItemBase
  {
    var orchardContentItem = await content.GetAsync(id);
    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> GetContentAsync<T>(
    this IContentManager content,
    string id,
    VersionOptions options
  ) where T : ContentItemBase
  {
    var orchardContentItem = await content.GetAsync(id, options);
    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> GetVersionedContentAsync<T>(
    this IContentManager content,
    string id
  ) where T : ContentItemBase
  {
    var orchardContentItem = await content.GetVersionAsync(id);
    return orchardContentItem.AsContent<T>();
  }

  public static async Task<IEnumerable<T>> GetContentAsync<T>(
    this IContentManager content,
    params string[] ids
  ) where T : ContentItemBase
  {
    var orchardContentItems = await content.GetAsync(ids);
    return orchardContentItems
      .Select(orchardContentItem => orchardContentItem.AsContent<T>())
      .WhereNotNull();
  }

  internal static T PopulateContent<T>(this T content) where T : ContentItemBase
  {
    foreach (var property in typeof(T).GetProperties())
    {
      if (
        !property.PropertyType.IsGenericType
        || !Equals(
          property.PropertyType.GetGenericTypeDefinition(),
          typeof(Lazy<>)
        )
      )
      {
        continue;
      }

      var partType = property.PropertyType
        .GetGenericArguments()
        .FirstOrDefault();
      if (partType is null || !partType.IsAssignableTo(typeof(ContentElement)))
      {
        continue;
      }

      var lazy = content.Inner.CreateLazy(partType, property.Name);
      content.SetFieldOrPropertyValue(property.Name, lazy);
    }

    return content;
  }

  private static object CreateLazy(
    this OrchardContentItem contentItem,
    Type partType,
    string partName
  )
  {
    var constructor = typeof(Lazy<>)
      .MakeGenericType(new[] { partType })
      .GetConstructor(
        new[] { typeof(Func<>).MakeGenericType(new[] { partType }) }
      );
    if (constructor is null)
    {
      throw new InvalidOperationException(
        $"Could not find constructor for {typeof(Lazy<>).MakeGenericType(new[] { partType })}"
      );
    }

    return constructor.Invoke(
      new[]
      {
        LazyFactoryCaster
          .MakeGenericMethod(new[] { typeof(ContentElement), partType })
          .Invoke(
            null,
            new[]
            {
              () =>
                contentItem.Get(partType, partName)
                ?? contentItem.Get(partType, partName + "Part")
            }
          )
      }
    );
  }

  private static readonly MethodInfo LazyFactoryCaster =
    typeof(FunctionCastExtensions)
      .GetMethods()
      .Where(
        method =>
          method.Name == nameof(FunctionCastExtensions.Cast)
          && method.GetGenericArguments().Length == 2
      )
      .First();
}

internal interface IContentTypeBaseDerivedIndicator { }
