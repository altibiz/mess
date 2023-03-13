using System.Security.Cryptography;
using System.Text;

namespace Mess.System.Extensions.Strings;

public static class StringHashExtensions
{
  public static string GetSha256Hash(this string @string)
  {
    using var crypt = SHA256.Create();
    string hash = string.Empty;
    byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(@string));
    foreach (byte theByte in crypto)
    {
      hash += theByte.ToString("x2");
    }
    return hash;
  }
}
