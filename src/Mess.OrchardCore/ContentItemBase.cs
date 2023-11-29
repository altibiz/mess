using System.Reflection;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using Mess.System.Extensions.Objects;
using Mess.System.Extensions.Functions;
using Mess.System.Extensions.Strings;
using Mess.System.Extensions.Enumerables;
using System.Linq.Expressions;
using YesSql;

namespace Mess.OrchardCore;

public abstract class ContentItemBase
{
  protected ContentItemBase(ContentItem contentItem)
  {
    Inner = contentItem;
  }

  public ContentItem Inner { get; init; }

  public Lazy<ContainedPart> ContainedPart { get; init; } = default!;

  public static implicit operator ContentItem(ContentItemBase @this)
  {
    return @this.Inner;
  }

  public string ContentItemId => Inner.ContentItemId;
}

// NOTE: don't use for now - needs more testing
public abstract class ContentItemBase<TDerived>
  : ContentItemBase,
    IContentTypeBaseDerivedIndicator
  where TDerived : ContentItemBase<TDerived>
{
  protected ContentItemBase(ContentItem item)
    : base(item)
  {
    (this as TDerived)!.PopulateContent();
  }
}

public static class ContentItemExtensions
{
  public static string ContentTypeName(this Type @this) =>
    @this.Name.RegexRemove("Item$");

  public static string ContentTypeName<T>() => typeof(T).ContentTypeName();

  public static T AsContent<T>(this ContentItem @this)
    where T : ContentItemBase
  {
    var contentTypeName = ContentTypeName<T>();
    if (@this.ContentType != contentTypeName)
    {
      throw new InvalidOperationException(
        $"Content type mismatch: expected {contentTypeName}, got {@this.ContentType}"
      );
    }

    var contentItem = (T?)
      Activator.CreateInstance(
        typeof(T),
        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
        null,
        new[] { @this },
        null
      );

    return contentItem is null
      || contentItem
        .GetType()
        .IsAssignableTo(typeof(IContentTypeBaseDerivedIndicator))
      ? throw new InvalidOperationException(
        $"Could not create content item of type {typeof(T)}"
      )
      : contentItem.PopulateContent();
  }

  public static async Task<TItem> AlterAsync<TItem, TPart>(
    this TItem @this,
    Expression<Func<TItem, Lazy<TPart>>> expression,
    Func<TPart, Task> action
  )
    where TItem : ContentItemBase
    where TPart : ContentPart, new()
  {
    var property = GetProperty(@this, expression);
    var part = @this.Inner.GetOrCreate<TPart>();
    await action(part);
    @this.Inner.Apply(part);
    PopulateContent(@this, property);
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
    var property = GetProperty(@this, expression);
    var part = @this.Inner.GetOrCreate<TPart>();
    action(part);
    @this.Inner.Apply(part);
    PopulateContent(@this, property);
    return @this;
  }

  public static async Task<T> NewContentAsync<T>(this IContentManager content)
    where T : ContentItemBase
  {
    var orchardContentItem = await content.NewAsync(
      typeof(T).ContentTypeName()
    ) ?? throw new InvalidOperationException(
        $"Could not create content item of type {typeof(T)}"
      );

    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> CloneContentAsync<T>(
    this IContentManager content,
    ContentItem item
  )
    where T : ContentItemBase
  {
    var orchardContentItem = await content.CloneAsync(item) ?? throw new InvalidOperationException(
        $"Could not clone content item of type {typeof(T)}"
      );

    return orchardContentItem.AsContent<T>();
  }

  public static async Task<T?> GetContentAsync<T>(
    this IContentManager content,
    string id
  )
    where T : ContentItemBase
  {
    var orchardContentItem = await content.GetAsync(id);

    return orchardContentItem?.AsContent<T>();
  }

  public static async Task<T?> GetContentAsync<T>(
    this IContentManager content,
    string id,
    VersionOptions options
  )
    where T : ContentItemBase
  {
    var orchardContentItem = await content.GetAsync(id, options);

    return orchardContentItem?.AsContent<T>();
  }

  public static async Task<T?> GetVersionedContentAsync<T>(
    this IContentManager content,
    string id
  )
    where T : ContentItemBase
  {
    var orchardContentItem = await content.GetVersionAsync(id);

    return orchardContentItem?.AsContent<T>();
  }

  public static async Task<IEnumerable<T>> GetContentAsync<T>(
    this IContentManager content,
    params string[] ids
  )
    where T : ContentItemBase
  {
    var orchardContentItems = await content.GetAsync(ids);

    return orchardContentItems is null
      ? Enumerable.Empty<T>()
      : orchardContentItems
      .Select(orchardContentItem => orchardContentItem.AsContent<T>())
      .WhereNotNull();
  }

  public static async Task<IEnumerable<T>> ListContentAsync<T>(
    this IQuery<ContentItem> query
  )
    where T : ContentItemBase
  {
    var items = await query.ListAsync();
    return items.Select(item => item.AsContent<T>());
  }

  public static async Task<T?> FirstOrDefaultContentAsync<T>(
    this IQuery<ContentItem> query
  )
    where T : ContentItemBase
  {
    var items = await query.FirstOrDefaultAsync();

    return items?.AsContent<T>();
  }

  internal static PropertyInfo GetProperty<TItem, TPart>(
    this TItem _,
    Expression<Func<TItem, Lazy<TPart>>> expression
  )
    where TItem : ContentItemBase
  {
    return expression.Body is not MemberExpression memberExpression
      ? throw new InvalidOperationException(
        $"Expression {expression} is not a member expression"
      )
      : memberExpression.Member is not PropertyInfo property
      ? throw new InvalidOperationException(
        $"Expression {expression} is not a property expression"
      )
      : property;
  }

  internal static T PopulateContent<T>(this T content)
    where T : ContentItemBase
  {
    foreach (var property in typeof(T).GetProperties())
    {
      PopulateContent(content, property);
    }

    return content;
  }

  internal static void PopulateContent<T>(this T content, PropertyInfo property)
    where T : ContentItemBase
  {
    if (
      !property.PropertyType.IsGenericType
      || !Equals(
        property.PropertyType.GetGenericTypeDefinition(),
        typeof(Lazy<>)
      )
    )
    {
      return;
    }

    var partType = property.PropertyType.GetGenericArguments().FirstOrDefault();
    if (partType is null || !partType.IsAssignableTo(typeof(ContentElement)))
    {
      return;
    }

    var lazy = CreateLazy(content.Inner, partType, property.Name);
    content.SetFieldOrPropertyValue(property.Name, lazy);
  }

  private static object CreateLazy(
    ContentItem contentItem,
    Type partType,
    string partName
  )
  {
    var constructor = typeof(Lazy<>)
      .MakeGenericType(new[] { partType })
      .GetConstructor(
        new[] { typeof(Func<>).MakeGenericType(new[] { partType }) }
      ) ?? throw new InvalidOperationException(
        $"Could not find constructor for {typeof(Lazy<>).MakeGenericType(new[] { partType })}"
      );

    var lazy = constructor.Invoke(
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

    return lazy;
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
