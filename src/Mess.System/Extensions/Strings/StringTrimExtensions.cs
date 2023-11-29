using System.Globalization;

namespace Mess.System.Extensions.Strings;

public static class StringTrimExtensions
{
  public static string TrimStart(this string @this, string trimString)
  {
    string result = @this;
    while (result.StartsWith(trimString, StringComparison.InvariantCulture))
    {
      result = result[trimString.Length..];
    }

    return result;
  }

  public static string TrimEnd(this string @this, string trimString)
  {
    string result = @this;
    while (result.EndsWith(trimString, StringComparison.InvariantCulture))
    {
      result = result[0..(result.Length - trimString.Length)];
    }

    return result;
  }

  public static string Trim(this string @this, string trimString) =>
    @this.TrimStart(trimString).TrimEnd(trimString);
}
