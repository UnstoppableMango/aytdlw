using System.Diagnostics;
using System.Reactive.Linq;

namespace Aytdlw.Service.Models;

public record DownloadJob
{
    private readonly Process _process;
    private readonly Lazy<IObservable<DataReceivedEventArgs>> _outputReceived;
    private readonly Lazy<IObservable<DataReceivedEventArgs>> _errorReceived;
    private readonly Lazy<IObservable<EventArgs>> _exited;

    public DownloadJob(Process process)
    {
        _process = process;
        var temp = process.StandardOutput;
        
        _outputReceived = new(
            () => Observable.FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(
                    handler => process.OutputDataReceived += handler,
                    handler => process.OutputDataReceived -= handler)
                .Select(x => x.EventArgs));

        _errorReceived = new(
            () => Observable.FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(
                    handler => process.ErrorDataReceived += handler,
                    handler => process.ErrorDataReceived -= handler)
                .Select(x => x.EventArgs));

        _exited = new(
            () => Observable.FromEventPattern<EventHandler, EventArgs>(
                    handler => process.Exited += handler,
                    handler => process.Exited -= handler)
                .Select(x => x.EventArgs));
    }

    public IObservable<DataReceivedEventArgs> OutputReceived => _outputReceived.Value;

    public IObservable<DataReceivedEventArgs> ErrorReceived => _errorReceived.Value;

    public IObservable<EventArgs> Exited => _exited.Value;

    public Task WaitUntilFinished(CancellationToken cancellationToken = default)
    {
        return _process.WaitForExitAsync(cancellationToken);
    }
}
