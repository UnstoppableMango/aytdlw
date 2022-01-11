namespace Aytdlw.Service.Services;

public class QueueProcessor : BackgroundService
{
    private readonly ITaskQueue _queue;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public QueueProcessor(ITaskQueue queue, IServiceProvider services, ILogger logger)
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
            _logger.LogInformation("Processing new work item");
            await work(scope.ServiceProvider, stoppingToken);
        }
    }
}
