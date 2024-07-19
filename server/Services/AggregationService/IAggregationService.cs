namespace project_frej.Services;
public interface IAggregationService
{
    public interface IAggregationService
    {
        Task AggregateHourlyDataAsync(DateTime date, int hour);
        Task AggregateDailyDataAsync(DateTime date);
        Task FillBlankAggregationsAsync();
    }
}
