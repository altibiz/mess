using System.Text.RegularExpressions;

namespace Mess.Prelude.Extensions.Strings;

public static class StringRegexExtensions
{
  public static string RegexReplace(
    this string str,
    Regex pattern,
    string replacement
  )
  {
    return pattern.Replace(str, replacement);
  }

  public static string RegexReplace(
    this string str,
    string pattern,
    string replacement
  )
  {
    return str.RegexReplace(new Regex(pattern), replacement);
  }

  public static string RegexReplace(
    this string str,
    string pattern,
    string replacement,
    RegexOptions options
  )
  {
    return str.RegexReplace(new Regex(pattern, options), replacement);
  }

  public static string RegexRemove(this string str, Regex pattern)
  {
    return str.RegexReplace(pattern, "");
  }

  public static string RegexRemove(this string str, string pattern)
  {
    return str.RegexReplace(pattern, "");
  }

  public static string RegexRemove(
    this string str,
    string pattern,
    RegexOptions options
  )
  {
    return str.RegexReplace(pattern, "", options);
  }

  public static bool RegexMatch(this string str, Regex pattern)
  {
    return pattern.IsMatch(str);
  }

  public static bool RegexMatch(this string str, string pattern)
  {
    return new Regex(pattern).IsMatch(str);
  }

  public static bool RegexMatch(
    this string str,
    string pattern,
    RegexOptions options
  )
  {
    return new Regex(pattern, options).IsMatch(str);
  }
}
