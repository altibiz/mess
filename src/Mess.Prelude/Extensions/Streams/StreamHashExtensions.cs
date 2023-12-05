using System.Globalization;
using System.Security.Cryptography;
using HashDepot;

namespace Mess.Prelude.Extensions.Streams;

public static class StreamHashExtensions
{
  private static readonly uint MurMurHashSeed = 42; // ( ͡° ͜ʖ ͡°)

  public static string GetSha256Hash(this Stream stream)
  {
    using var crypt = SHA256.Create();
    var hash = string.Empty;
    var crypto = crypt.ComputeHash(stream);
    foreach (var theByte in crypto)
      hash += theByte.ToString("x2", CultureInfo.InvariantCulture);
    return hash;
  }

  public static string GetMurMurHash(this Stream stream)
  {
    var hash = string.Empty;
    var crypto = MurmurHash3.Hash128(stream, MurMurHashSeed);
    foreach (var theByte in crypto)
      hash += theByte.ToString("x2", CultureInfo.InvariantCulture);
    return hash;
  }
}
