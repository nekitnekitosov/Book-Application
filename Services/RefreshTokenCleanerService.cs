namespace Book
{
    public class RefreshTokenCleanerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RefreshTokenCleanerService> _logger;
        public RefreshTokenCleanerService(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Сервис по очистке токенов запущен!");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();

                        var deletedCount = await tokenRepository.DeleteExpireRefreshTokensAsync();
                        _logger.LogInformation($"Удалено {deletedCount} токенов");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}