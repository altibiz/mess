namespace Mess.System.Extensions.Enumerable;

public static class EnumerableWhereNotNullExtensions
{
  public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
    where T : class => @this.Where(@object => @object is not null).Cast<T>();

  public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
    where T : struct =>
    @this
      .Where(@object => @object is not null)
#nullable disable
      .Select(@object => @object.Value)
#nullable enable
      .Cast<T>();
}
