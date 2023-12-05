using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Fields;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Mess.Iot.Security;

// https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing

public class ApiKeyFieldService : IApiKeyFieldService
{
  public byte[] GenerateApiKeySalt()
  {
    var salt = RandomNumberGenerator.GetBytes(128 / 8);

    return salt;
  }

  public async Task<byte[]> GenerateApiKeySaltAsync()
  {
    var salt = RandomNumberGenerator.GetBytes(128 / 8);

    return salt;
  }

  public async Task<string> HashApiKeyAsync(string apiKey, byte[] salt)
  {
    var hashed = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        apiKey!,
        salt,
        KeyDerivationPrf.HMACSHA256,
        100000,
        256 / 8
      )
    );

    return hashed;
  }

  public string HashApiKey(string apiKey, byte[] salt)
  {
    var hashed = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        apiKey!,
        salt,
        KeyDerivationPrf.HMACSHA256,
        100000,
        256 / 8
      )
    );

    return hashed;
  }

  public bool Authorize(ApiKeyField apiKeyField, string apiKey)
  {
    var hashedApiKey = HashApiKey(apiKey, apiKeyField.Salt);

    var authorized = CryptographicOperations.FixedTimeEquals(
      MemoryMarshal.Cast<char, byte>(hashedApiKey.AsSpan()),
      MemoryMarshal.Cast<char, byte>(apiKeyField.Hash.AsSpan())
    );

    return authorized;
  }

  public async Task<bool> AuthorizeAsync(ApiKeyField apiKeyField, string apiKey)
  {
    var hashedApiKey = HashApiKey(apiKey, apiKeyField.Salt);

    var authorized = CryptographicOperations.FixedTimeEquals(
      MemoryMarshal.Cast<char, byte>(hashedApiKey.AsSpan()),
      MemoryMarshal.Cast<char, byte>(apiKeyField.Hash.AsSpan())
    );

    return authorized;
  }
}
