using System.Diagnostics;
using System.Reactive.Linq;

using Aytdlw.Service.Models;

using Microsoft.Extensions.Options;

namespace Aytdlw.Service.Services;

public class YtdlpProcess : IYoutubeDl
{
    private readonly IOptions<AytdlwOptions> _options;

    public YtdlpProcess(IOptions<AytdlwOptions> options)
    {
        _options = options;
    }

    public ValueTask<DownloadJob> DownloadAsync(string url, CancellationToken cancellationToken = default)
    {
        var arguments = _options.Value.GetProcessArguments().ToList();

        arguments.Add(url);

        var startInfo = new ProcessStartInfo {
            // FileName = "yt-dlp", // TODO: Allow configuring
            FileName = "/home/erik/src/repos/unmango/loop.sh",
            Arguments = string.Join(' ', arguments),
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        var process = new Process {
            EnableRaisingEvents = true,
            StartInfo = startInfo,
        };

        cancellationToken.Register(state => {
            var innerProcess = (Process)state!;
            if (!innerProcess.HasExited) innerProcess.Kill();
        }, process);

        if (!process.Start()) {
            return ValueTask.FromException<DownloadJob>(new("Unable to start process"));
        }
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return new(new DownloadJob(process));
    }
}
