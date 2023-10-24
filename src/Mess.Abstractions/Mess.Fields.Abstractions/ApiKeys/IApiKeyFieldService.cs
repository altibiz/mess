using Mess.Fields.Abstractions.Fields;

namespace Mess.Fields.Abstractions.ApiKeys;

public interface IApiKeyFieldService
{
  Task<byte[]> GenerateApiKeySaltAsync();

  byte[] GenerateApiKeySalt();

  Task<string> HashApiKeyAsync(string apiKey, byte[] salt);

  string HashApiKey(string apiKey, byte[] salt);

  bool Authorize(ApiKeyField apiKeyField, string apiKey);

  Task<bool> AuthorizeAsync(ApiKeyField apiKeyField, string apiKey);
}
