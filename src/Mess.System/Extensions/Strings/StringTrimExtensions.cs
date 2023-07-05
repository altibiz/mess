namespace Mess.System.Extensions.Strings;

public static class StringTrimExtensions
{
  public static string TrimStart(this string @string, string trimString)
  {
    string result = @string;
    while (result.StartsWith(trimString))
    {
      result = result.Substring(trimString.Length);
    }

    return result;
  }

  public static string TrimEnd(this string @string, string trimString)
  {
    string result = @string;
    while (result.EndsWith(trimString))
    {
      result = result.Substring(0, result.Length - trimString.Length);
    }

    return result;
  }

  public static string Trim(this string @string, string trimString) =>
    @string.TrimStart(trimString).TrimEnd(trimString);
}
