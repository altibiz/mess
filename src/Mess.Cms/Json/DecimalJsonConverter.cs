using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class DecimalJsonConverter : JsonConverter
{
  public override bool CanConvert(Type objectType)
  {
    return objectType == typeof(decimal) || objectType == typeof(decimal?);
  }

  public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
  {
    JToken token = JToken.Load(reader);
    if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
    {
      return token.ToObject<decimal>();
    }
    if (token.Type == JTokenType.String)
    {
      return decimal.Parse(token.ToString(), CultureInfo.InvariantCulture);
    }
    if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
    {
      return null;
    }

    throw new JsonSerializationException("Unexpected token type: " + token.Type.ToString());
  }

  public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
  {
    throw new NotImplementedException();
  }
}
