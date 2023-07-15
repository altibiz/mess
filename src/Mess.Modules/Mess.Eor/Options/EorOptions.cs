namespace Mess.Eor.Options;

public class EorOptions
{
  public DevelopmentOptions Development { get; set; } = new();

  public class DevelopmentOptions
  {
    public string DeviceId { get; set; } = "eor";
    public string ApiKey { get; set; } = "eor";
  }
}
