namespace Mess.Eor.Abstractions.Client;

public interface IEorTimeseriesClient : IEorTimeseriesQuery
{
  public void AddEorMeasurement(EorMeasurement model);

  public Task AddEorMeasurementAsync(EorMeasurement model);

  public void AddEorStatus(EorStatus model);

  public Task AddEorStatusAsync(EorStatus model);
}
