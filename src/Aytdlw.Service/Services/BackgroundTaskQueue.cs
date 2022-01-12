using System.Reactive.Subjects;
using System.Threading.Channels;

namespace Aytdlw.Service.Services;

public class BackgroundTaskQueue : ITaskQueue
{
    private readonly Channel<Func<IServiceProvider, CancellationToken, ValueTask>> _channel;
    private readonly Subject<int> _onEnqueue = new();
    private readonly Subject<int> _onDequeue = new();
    private int _currentId;

    public BackgroundTaskQueue()
    {
        var options = new BoundedChannelOptions(1) {
            FullMode = BoundedChannelFullMode.Wait,
        };

        _channel = Channel.CreateBounded<Func<IServiceProvider, CancellationToken, ValueTask>>(options);
    }

    public IObservable<int> OnEnqueue => _onEnqueue;

    public IObservable<int> OnDequeue => _onDequeue;

    public async ValueTask<int> EnqueueAsync(
        Func<IServiceProvider, CancellationToken, ValueTask> work,
        CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(work, cancellationToken);
        var id = ++_currentId;
        _onEnqueue.OnNext(id);
        return id;
    }

    public ValueTask<Func<IServiceProvider, CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }
}
