namespace Mess.System.Extensions.Enums;

public static class EnumConversionExtensions
{
  public static bool ToBool<T>(this T @this, T trueValue)
    where T : Enum
  {
    if (Convert.ToInt32(@this) == Convert.ToInt32(trueValue))
    {
      return true;
    }

    return false;
  }
}
