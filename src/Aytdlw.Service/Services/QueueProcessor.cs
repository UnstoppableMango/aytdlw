namespace Aytdlw.Service.Services;

public class QueueProcessor : BackgroundService
{
    private readonly ITaskQueue _queue;
    private readonly IServiceProvider _services;
    private readonly ILogger<QueueProcessor> _logger;

    public QueueProcessor(ITaskQueue queue, IServiceProvider services, ILogger<QueueProcessor> logger)
    {
        _queue = queue;
        _services = services;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Beginning work loop");
        while (!stoppingToken.IsCancellationRequested) {
            _logger.LogDebug("Creating service scope");
            await using var scope = _services.CreateAsyncScope();
            _logger.LogDebug("Dequeuing work item");
            var work = await _queue.DequeueAsync(stoppingToken);

            try {
                _logger.LogInformation("Processing new work item");
                await work(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception exception) {
                _logger.LogError(exception, "Exception processing work");
            }
        }
    }
}
