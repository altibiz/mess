namespace Mess.MeasurementDevice.Abstractions.Dispatchers;

public interface IMeasurementDispatcher
{
  public string Id { get; }

  public void Dispatch(string measurement);

  public Task DispatchAsync(string measurement);
}
