namespace Mess.Prelude.Extensions.Strings;

public static class StringTrimExtensions
{
  public static string TrimStart(this string @this, string trimString)
  {
    var result = @this;
    while (result.StartsWith(trimString, StringComparison.InvariantCulture))
      result = result[trimString.Length..];

    return result;
  }

  public static string TrimEnd(this string @this, string trimString)
  {
    var result = @this;
    while (result.EndsWith(trimString, StringComparison.InvariantCulture))
      result = result[..(result.Length - trimString.Length)];

    return result;
  }

  public static string Trim(this string @this, string trimString)
  {
    return @this.TrimStart(trimString).TrimEnd(trimString);
  }
}
