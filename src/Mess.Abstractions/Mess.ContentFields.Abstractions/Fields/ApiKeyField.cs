using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Mess.ContentFields.Abstractions.Fields;

public class ApiKeyField : ContentField
{
  [JsonIgnore]
  public string Value { get; set; } = default!;

  public string Hash { get; set; } = default!;

  public byte[] Salt { get; set; } = default!;
}
