using System.Globalization;

namespace Mess.Eor.Localization.Abstractions;

public interface IEorLocalizer
{
  public string this[string notLocalized] { get; }

  public string ForCulture(CultureInfo culture, string notLocalized);

  public string FuckThisShit(CultureInfo culture);
}
