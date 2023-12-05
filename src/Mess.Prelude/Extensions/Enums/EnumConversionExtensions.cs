using System.Globalization;

namespace Mess.Prelude.Extensions.Enums;

public static class EnumConversionExtensions
{
  public static bool ToBool<T>(this T @this, T trueValue)
    where T : Enum
  {
    return Convert.ToInt32(@this, CultureInfo.InvariantCulture) ==
           Convert.ToInt32(trueValue, CultureInfo.InvariantCulture);
  }
}
