using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public interface ITaskQueue
{
    IObservable<int> OnEnqueue { get; }

    IObservable<int> OnDequeue { get; }

    ValueTask<int> EnqueueAsync(
        Func<IServiceProvider, CancellationToken, ValueTask<DownloadJob>> work,
        CancellationToken cancellationToken);

    ValueTask<Func<IServiceProvider, CancellationToken, ValueTask<DownloadJob>>> DequeueAsync(
        CancellationToken cancellationToken);
}
