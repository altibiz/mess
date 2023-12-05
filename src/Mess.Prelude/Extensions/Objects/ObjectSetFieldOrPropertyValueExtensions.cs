using System.Reflection;

namespace Mess.Prelude.Extensions.Objects;

public static class ObjectSetFieldOrPropertyValueExtensions
{
  public static TValue? SetFieldOrPropertyValue<TObject, TValue>(
    this TObject @this,
    string fieldOrPropertyName,
    TValue value
  )
  {
    if (@this is null) throw new ArgumentNullException(nameof(@this));

    var type = @this.GetType();
    return SetFieldValue(@this, type, fieldOrPropertyName, value);
  }

  private static TValue? SetFieldValue<TObject, TValue>(
    this TObject @this,
    Type type,
    string fieldOrPropertyName,
    TValue value
  )
  {
    var field = type.GetField(
      fieldOrPropertyName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
    );
    if (field is null)
      return SetPropertyValue(@this, type, fieldOrPropertyName, value);

    var previous = field.GetValue(@this);
    field.SetValue(@this, value);

    return previous is null ? default : (TValue)previous;
  }

  private static TValue? SetPropertyValue<TObject, TValue>(
    this TObject @this,
    Type type,
    string fieldOrPropertyName,
    TValue value
  )
  {
    var property = type.GetProperty(
      fieldOrPropertyName,
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
    );
    if (property is null) return default;

    var previous = property.GetValue(@this);
    property.SetValue(@this, value);

    return previous is null ? default : (TValue)previous;
  }
}
