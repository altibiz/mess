using System.Text.RegularExpressions;

namespace Mess.System.Extensions.String;

public static partial class RegexExtensions
{
  public static string RegexReplace(
    this string str,
    Regex pattern,
    string replacement
  ) => pattern.Replace(str, replacement);

  public static string RegexReplace(
    this string str,
    string pattern,
    string replacement
  ) => str.RegexReplace(new Regex(pattern), replacement);

  public static string RegexReplace(
    this string str,
    string pattern,
    string replacement,
    RegexOptions options
  ) => str.RegexReplace(new Regex(pattern, options), replacement);

  public static string RegexRemove(this string str, Regex pattern) =>
    str.RegexReplace(pattern, "");

  public static string RegexRemove(this string str, string pattern) =>
    str.RegexReplace(pattern, "");
}
