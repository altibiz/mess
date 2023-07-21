using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.Abstractions.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Mess.MeasurementDevice.Security;

// https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing

public class ApiKeyFieldService : IApiKeyFieldService
{
  public byte[] GenerateApiKeySalt()
  {
    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

    return salt;
  }

  public async Task<byte[]> GenerateApiKeySaltAsync()
  {
    byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

    return salt;
  }

  public async Task<string> HashApiKeyAsync(string apiKey, byte[] salt)
  {
    string hashed = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        password: apiKey!,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8
      )
    );

    return hashed;
  }

  public string HashApiKey(string apiKey, byte[] salt)
  {
    string hashed = Convert.ToBase64String(
      KeyDerivation.Pbkdf2(
        password: apiKey!,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8
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
