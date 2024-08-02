namespace project_frej.Services;

/// <summary>
/// Inteface for a service aggregating sensor data.
/// </summary>
public interface IAggregationService
{
    /// <summary>
    /// Aggregates hourly sensor data for the specified date and hour and saves to the database.
    /// </summary>
    /// <param name="date">The date of the data to aggregate.</param>
    /// <param name="hour">The hour of the data to aggregate.</param>
    Task AggregateHourlyDataAsync(DateTime date, int hour);
    /// <summary>
    /// Aggregates daily sensor data for the specified date and saves to database.
    /// </summary>
    /// <param name="date">The date of the data to aggregate.</param>

    Task AggregateDailyDataAsync(DateTime date);
    /// <summary>
    /// Fills blank aggregations for any missing hourly and daily data and saves to the database.
    /// </summary>
    Task FillBlankAggregationsAsync();
}
