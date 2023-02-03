namespace Mess.System.Extensions.IEnumerable;

public static class IEnumerableIndexExtensions
{
  public static IEnumerable<(T item, int index)> Index<T>(
    this IEnumerable<T> @this
  ) => @this.Select((item, index) => (item, index));

  public static IEnumerable<int> Indices<T>(this IEnumerable<T> @this) =>
    @this.Select((item, index) => index);
}
