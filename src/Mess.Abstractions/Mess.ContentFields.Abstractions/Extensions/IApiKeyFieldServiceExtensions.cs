using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.Abstractions.Services;

namespace Mess.ContentFields.Abstractions.Extensions;

public static class IApiKeyFieldServiceExtensions
{
  public static ApiKeyField HashApiKeyField(
    this IApiKeyFieldService apiKeyFieldService,
    string apiKey
  )
  {
    var apiKeySalt = apiKeyFieldService.GenerateApiKeySalt();
    var apiKeyHash = apiKeyFieldService.HashApiKey(apiKey, apiKeySalt);
    return new ApiKeyField { Salt = apiKeySalt, Hash = apiKeyHash };
  }

  public static async Task<ApiKeyField> HashApiKeyFieldAsync(
    this IApiKeyFieldService apiKeyFieldService,
    string apiKey
  )
  {
    var apiKeySalt = await apiKeyFieldService.GenerateApiKeySaltAsync();
    var apiKeyHash = await apiKeyFieldService.HashApiKeyAsync(
      apiKey,
      apiKeySalt
    );
    return new ApiKeyField { Salt = apiKeySalt, Hash = apiKeyHash };
  }
}
