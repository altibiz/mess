namespace Mess.Eor.Abstractions.Client;

public interface IEorTimeseriesQuery
{
  public Task<IReadOnlyList<EorMeasurement>> GetEorMeasurementsAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<EorMeasurement> GetEorMeasurements(
    string source,
    DateTime beginning,
    DateTime end
  );

  public Task<IReadOnlyList<EorStatus>> GetEorStatusesAsync(
    string source,
    DateTime beginning,
    DateTime end
  );

  public IReadOnlyList<EorStatus> GetEorStatuses(
    string source,
    DateTime beginning,
    DateTime end
  );

  public Task<(
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  )> GetEorDataAsync(string source, DateTime beginning, DateTime end);

  public (
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  ) GetEorData(string source, DateTime beginning, DateTime end);

  public Task<
    IReadOnlyList<EorMeasurementDeviceSummary>
  > GetEorMeasurementDeviceSummariesAsync(IReadOnlyCollection<string> sources);

  public IReadOnlyList<EorMeasurementDeviceSummary> GetEorMeasurementDeviceSummaries(
    IReadOnlyCollection<string> sources
  );

  public Task<EorMeasurement?> GetLastEorMeasurementAsync(string source);

  public EorMeasurement? GetLastEorMeasurement(string source);

  public Task<EorStatus?> GetEorStatusAsync(string source);

  public EorStatus? GetEorStatus(string source);

  public Task<EorMeasurementDeviceSummary?> GetEorMeasurementDeviceSummaryAsync(
    string source
  );

  public EorMeasurementDeviceSummary? GetEorMeasurementDeviceSummary(
    string source
  );
}
