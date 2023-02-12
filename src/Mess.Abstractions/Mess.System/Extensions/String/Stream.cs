using System.Text;

namespace Mess.System.Extensions.String;

public static class StreamExtensions
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
    return await reader.ReadToEndAsync();
  }
}
