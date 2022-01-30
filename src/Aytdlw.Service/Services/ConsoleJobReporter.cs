using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public sealed class ConsoleJobReporter : IJobReporter, IDisposable
{
    private readonly ILogger<ConsoleJobReporter> _logger;
    private IDisposable? _outputSubscription;
    private IDisposable? _errorSubscription;
    private IDisposable? _exitedSubscription;

    public ConsoleJobReporter(ILogger<ConsoleJobReporter> logger)
    {
        _logger = logger;
    }

    public void Watch(DownloadJob job)
    {
        _outputSubscription =
            job.OutputReceived.Subscribe(args => _logger.LogInformation("{Data}", args.Data ?? "Output event w/o data"));
        
        _errorSubscription =
            job.ErrorReceived.Subscribe(args => _logger.LogInformation("{Data}", args.Data ?? "Error event w/o data"));

        _exitedSubscription = job.Exited.Subscribe(_ => _logger.LogInformation("Process exited"));
    }

    public void Dispose()
    {
        _outputSubscription?.Dispose();
        _errorSubscription?.Dispose();
        _exitedSubscription?.Dispose();
    }
}
