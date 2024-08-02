namespace project_frej.Services;

/// <summary>
/// Hosted service responsible for aggregating sensor data on an hourly and daily basis.
/// </summary>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="serviceScopeFactory">The service scope factory to create service scopes.</param>
/// <remarks>
/// Initializes a new instance of the <see cref="AggregationHostedService"/> class.
/// </remarks>
/// <param name="logger">The logger instance for logging information and errors.</param>
/// <param name="serviceScopeFactory">The service scope factory to create service scopes.</param>
public class AggregationHostedService(ILogger<AggregationHostedService> logger, IServiceScopeFactory serviceScopeFactory) : IHostedService, IDisposable
{
    private Timer? _hourlyTimer;
    private Timer? _dailyTimer;

    /// <summary>
    /// Starts the hosted service.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the start operation.</param>
    /// <returns>A task that represents the start operation.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Aggregation Service starting.");

        Task.Run(() => DoFillBlankAggregationsAsync(), cancellationToken);

        var nextHour = DateTime.Now.AddHours(1);
        nextHour = new DateTime(nextHour.Year, nextHour.Month, nextHour.Day, nextHour.Hour, 0, 0);
        var hourlyInterval = TimeSpan.FromHours(1);
        _hourlyTimer = new Timer(async state => await DoHourlyAggregationAsync(), null, nextHour - DateTime.Now, hourlyInterval);

        var nextMidnight = DateTime.Today.AddDays(1);
        var dailyInterval = TimeSpan.FromDays(1);
        _dailyTimer = new Timer(async state => await DoDailyAggregationAsync(), null, nextMidnight - DateTime.Now, dailyInterval);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the hosted service.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the stop operation.</param>
    /// <returns>A task that represents the stop operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Aggregation Service stopping.");

        _hourlyTimer?.Change(Timeout.Infinite, 0);
        _dailyTimer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the timers used by the hosted service.
    /// </summary>
    public void Dispose()
    {
        _hourlyTimer?.Dispose();
        _dailyTimer?.Dispose();

        GC.SuppressFinalize(this); // Prevents finalization of the object by the garbage collector.
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
}
