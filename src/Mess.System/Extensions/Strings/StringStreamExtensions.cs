using System.Text;

namespace Mess.System.Extensions.Strings;

public static class StringStreamExtensions
{
  public static string Encode(this Stream stream, Encoding? encoding = null)
  {
    encoding ??= Encoding.UTF8;
    using var reader = new StreamReader(stream, encoding);
    return reader.ReadToEnd();
  }

  public static async Task<string> EncodeAsync(
    this Stream stream,
    Encoding? encoding = null
  )
  {
    encoding ??= Encoding.UTF8;
    using var reader = new StreamReader(stream, encoding);
#if NET6_0
    var result = await Task.Run(() => reader.ReadToEnd());
#elif NET7_0
    var result = await reader.ReadToEndAsync();
#elif NET8_0
    var result = await reader.ReadToEndAsync();
#else
#error Wrong target framwork
#endif
    return result;
  }
}
