namespace project_frej.Services
{
    public class AggregationHostedService : BackgroundService
    {
        private readonly ILogger<AggregationHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AggregationHostedService(ILogger<AggregationHostedService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregation Hosted Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var aggregationService = scope.ServiceProvider.GetRequiredService<AggregationService>();

                    try
                    {
                        var now = DateTime.UtcNow;
                        await aggregationService.AggregateHourlyData(now, now.Hour);

                        if (now.Hour == 0)
                        {
                            await aggregationService.AggregateDailyData(now);
                        }

                        _logger.LogInformation("Aggregation completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while aggregating data.");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregation Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
