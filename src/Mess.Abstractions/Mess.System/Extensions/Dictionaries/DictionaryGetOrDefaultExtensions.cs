namespace Mess.System.Extensions.Dictionaries;

public static class DictionaryGetOrDefaultExtensions
{
  public static TOut? GetOrDefault<TIn, TOut>(
    this IDictionary<TIn, TOut> @this,
    TIn? key
  )
  {
    if (key is null)
    {
      return default(TOut);
    }

    try
    {
      return @this[key];
    }
    catch (KeyNotFoundException)
    {
      return default;
    }
  }

  public static TOut? GetOrDefaultValue<TIn, TOut>(
    this IDictionary<TIn, TOut> @this,
    TIn? key
  ) where TOut : struct
  {
    if (key is null)
    {
      return default(TOut);
    }

    try
    {
      return @this[key];
    }
    catch (KeyNotFoundException)
    {
      return default;
    }
  }
}
