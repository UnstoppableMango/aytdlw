using System.Diagnostics;
using System.Reactive.Linq;

namespace Aytdlw.Service.Models;

public record DownloadJob
{
    private readonly Lazy<IObservable<DataReceivedEventArgs>> _outputReceived;
    private readonly Lazy<IObservable<DataReceivedEventArgs>> _errorReceived;

    public DownloadJob(Process process)
    {
        _outputReceived = new(
            () => Observable.FromEvent<DataReceivedEventHandler, DataReceivedEventArgs>(
                handler => process.OutputDataReceived += handler,
                handler => process.OutputDataReceived -= handler));
        
        _errorReceived = new(
            () => Observable.FromEvent<DataReceivedEventHandler, DataReceivedEventArgs>(
                handler => process.ErrorDataReceived += handler,
                handler => process.ErrorDataReceived -= handler));
    }

    public IObservable<DataReceivedEventArgs> OutputReceived => _outputReceived.Value;

    public IObservable<DataReceivedEventArgs> ErrorReceived => _errorReceived.Value;
}
