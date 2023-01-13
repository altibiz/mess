namespace Mess.Xunit.Extensions.Npgsql;

public static class ConnectionStringExtensions
{
  public static readonly string CONNECTION_STRING_DATABASE_REGEX =
    @"Database=(.+?)(?:;|$)";
}
