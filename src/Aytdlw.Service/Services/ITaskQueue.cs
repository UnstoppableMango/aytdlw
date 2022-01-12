namespace Aytdlw.Service.Services;

public interface ITaskQueue
{
    IObservable<int> OnEnqueue { get; }

    IObservable<int> OnDequeue { get; }

    ValueTask<int> EnqueueAsync(
        Func<IServiceProvider, CancellationToken, ValueTask> work,
        CancellationToken cancellationToken);

    ValueTask<Func<IServiceProvider, CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);
}
