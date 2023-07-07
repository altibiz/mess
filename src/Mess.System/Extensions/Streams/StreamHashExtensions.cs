using System.Security.Cryptography;
using HashDepot;

namespace Mess.System.Extensions.Streams;

public static class StreamHashExtensions
{
  public static string GetSha256Hash(this Stream stream)
  {
    using var crypt = SHA256.Create();
    string hash = string.Empty;
    byte[] crypto = crypt.ComputeHash(stream);
    foreach (byte theByte in crypto)
    {
      hash += theByte.ToString("x2");
    }
    return hash;
  }

  public static string GetMurMurHash(this Stream stream)
  {
    var hash = string.Empty;
    var crypto = MurmurHash3.Hash128(stream, MurMurHashSeed);
    foreach (byte theByte in crypto)
    {
      hash += theByte.ToString("x2");
    }
    return hash;
  }

  private static readonly uint MurMurHashSeed = 42; // ( ͡° ͜ʖ ͡°)
}
