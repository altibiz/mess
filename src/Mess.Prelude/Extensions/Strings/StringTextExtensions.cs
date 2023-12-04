using System.Text;

namespace Mess.Prelude.Extensions.Strings;

public static class StringTextExtensions
{
  public static string ToAscii(this string text) =>
    Encoding.ASCII.GetString(
      Encoding.Convert(
        Encoding.UTF8,
        Encoding.GetEncoding(
          Encoding.ASCII.EncodingName,
          new EncoderReplacementFallback(string.Empty),
          new DecoderExceptionFallback()
        ),
        Encoding.UTF8.GetBytes(text)
      )
    );
}
