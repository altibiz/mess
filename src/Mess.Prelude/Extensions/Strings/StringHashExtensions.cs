using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mess.Prelude.Extensions.Strings;

public static class StringHashExtensions
{
  public static string GetSha256Hash(this string @this)
  {
    var hash = string.Empty;
    var crypto = SHA256.HashData(Encoding.UTF8.GetBytes(@this));
    foreach (var @byte in crypto)
      hash += @byte.ToString("x2", CultureInfo.InvariantCulture);
    return hash;
  }
}
