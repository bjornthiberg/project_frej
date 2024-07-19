namespace project_frej.Services;
public class AggregationHostedService(ILogger<AggregationHostedService> logger, IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
{
    private Timer? _hourlyTimer = null;
    private Timer? _dailyTimer = null;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Aggregation Service starting.");

        Task.Run(() => DoFillBlankAggregationsAsync(), cancellationToken);

        var nextHour = DateTime.Now.AddHours(1);
        nextHour = new DateTime(nextHour.Year, nextHour.Month, nextHour.Day, nextHour.Hour, 0, 0);
        var hourlyInterval = TimeSpan.FromHours(1);
        _hourlyTimer = new Timer(async state => await DoHourlyAggregationAsync(), null, nextHour - DateTime.Now, hourlyInterval);

        // Set up daily timer
        var nextMidnight = DateTime.Today.AddDays(1);
        var dailyInterval = TimeSpan.FromDays(1);
        _dailyTimer = new Timer(async state => await DoDailyAggregationAsync(), null, nextMidnight - DateTime.Now, dailyInterval);

        return Task.CompletedTask;
    }

    private async Task DoFillBlankAggregationsAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var aggregationService = scope.ServiceProvider.GetRequiredService<AggregationService>();
        try
        {
            await aggregationService.FillBlankAggregationsAsync();
            logger.LogInformation("Startup aggregation processing completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during startup aggregation processing.");
        }
    }

    private async Task DoHourlyAggregationAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var aggregationService = scope.ServiceProvider.GetRequiredService<AggregationService>();
        try
        {
            logger.LogInformation("Starting hourly aggregation processing.");
            var date = DateTime.Now.AddHours(-1);
            await aggregationService.AggregateHourlyDataAsync(date, date.Hour);
            logger.LogInformation("Hourly aggregation processing completed for date: {Date}, hour: {Hour}", date, date.Hour);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during hourly aggregation processing.");
        }
    }

    private async Task DoDailyAggregationAsync()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var aggregationService = scope.ServiceProvider.GetRequiredService<AggregationService>();
        try
        {
            logger.LogInformation("Starting daily aggregation processing.");
            var date = DateTime.Now.AddDays(-1);
            await aggregationService.AggregateDailyDataAsync(date);
            logger.LogInformation("Daily aggregation processing completed for date: {Date}", date);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during daily aggregation processing.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Aggregation Service stopping.");

        _hourlyTimer?.Change(Timeout.Infinite, 0);
        _dailyTimer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _hourlyTimer?.Dispose();
        _dailyTimer?.Dispose();

        GC.SuppressFinalize(this); // no clue what this does, but added because of CA1816 
    }

}

