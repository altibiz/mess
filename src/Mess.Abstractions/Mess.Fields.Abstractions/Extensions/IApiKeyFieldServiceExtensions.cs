using Mess.Fields.Abstractions.Fields;
using Mess.Fields.Abstractions.ApiKeys;

namespace Mess.Fields.Abstractions.Extensions;

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
