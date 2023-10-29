namespace Mess.Eor.Abstractions.Timeseries;

public interface IEorTimeseriesQuery
{
  public Task<IReadOnlyList<EorMeasurement>> GetEorMeasurementsAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<EorMeasurement> GetEorMeasurements(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<IReadOnlyList<EorStatus>> GetEorStatusesAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public IReadOnlyList<EorStatus> GetEorStatuses(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public Task<(
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  )> GetEorDataAsync(
    string source,
    DateTimeOffset fromDate,
    DateTimeOffset toDate
  );

  public (
    IReadOnlyList<EorStatus> Statuses,
    IReadOnlyList<EorMeasurement> Measurements
  ) GetEorData(string source, DateTimeOffset fromDate, DateTimeOffset toDate);

  public Task<IReadOnlyList<EorSummary>> GetEorIotDeviceSummariesAsync(
    IReadOnlyCollection<string> sources
  );

  public IReadOnlyList<EorSummary> GetEorIotDeviceSummaries(
    IReadOnlyCollection<string> sources
  );

  public Task<EorMeasurement?> GetLastEorMeasurementAsync(string source);

  public EorMeasurement? GetLastEorMeasurement(string source);

  public Task<EorStatus?> GetEorStatusAsync(string source);

  public EorStatus? GetEorStatus(string source);

  public Task<EorSummary?> GetEorIotDeviceSummaryAsync(string source);

  public EorSummary? GetEorIotDeviceSummary(string source);
}
