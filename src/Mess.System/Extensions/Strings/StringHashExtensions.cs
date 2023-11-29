using System.Security.Cryptography;
using System.Text;

namespace Mess.System.Extensions.Strings;

public static class StringHashExtensions
{
  public static string GetSha256Hash(this string @string)
  {
    string hash = string.Empty;
    byte[] crypto = SHA256.HashData(Encoding.UTF8.GetBytes(@string));
    foreach (byte @byte in crypto)
    {
      hash += @byte.ToString("x2");
    }
    return hash;
  }
}
