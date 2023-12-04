using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Mess.Prelude.Extensions.Strings;

public static class StringHashExtensions
{
  public static string GetSha256Hash(this string @this)
  {
    string hash = string.Empty;
    byte[] crypto = SHA256.HashData(Encoding.UTF8.GetBytes(@this));
    foreach (byte @byte in crypto)
    {
      hash += @byte.ToString("x2", CultureInfo.InvariantCulture);
    }
    return hash;
  }
}
