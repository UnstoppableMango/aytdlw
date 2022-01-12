using System.Diagnostics;

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
        var arguments = new List<string>();
        var options = _options.Value;

        if (!string.IsNullOrWhiteSpace(options.Format)) {
            arguments.Add($"-f {options.Format}");
        }

        if (options.AudioMultiStreams is not null) {
            arguments.Add("--audio-multistreams");
        }

        if (!string.IsNullOrWhiteSpace(options.MergeOutputFormat)) {
            arguments.Add($"--merge-output-format {options.MergeOutputFormat}");
        }

        if (!string.IsNullOrWhiteSpace(options.Username)) {
            arguments.Add($"--username {options.Username}");
        }

        if (!string.IsNullOrWhiteSpace(options.Password)) {
            arguments.Add($"--password {options.Password}");
        }

        if (!string.IsNullOrWhiteSpace(options.Cookies)) {
            arguments.Add($"--cookies {options.Cookies}");
        }

        if (options.EmbedSubs is not null) {
            arguments.Add($"--embed-subs {options.EmbedSubs}");
        }

        if (options.EmbedThumbnail is not null) {
            arguments.Add($"--embed-thumbnail {options.EmbedThumbnail}");
        }

        if (options.AddMetadata is not null) {
            arguments.Add($"--add-metadata {options.AddMetadata}");
        }

        if (!string.IsNullOrWhiteSpace(options.Output)) {
            arguments.Add($"--output {options.Output}");
        }
        
        arguments.Add(url);
        
        var startInfo = new ProcessStartInfo {
            FileName = "yt-dlp", // TODO: Allow configuring
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

        cancellationToken.Register((state) => {
            var innerProcess = (Process)state!;
            if (!innerProcess.HasExited) innerProcess.Kill();
        }, process);

        var job = new DownloadJob(process);
        
        return new ValueTask<DownloadJob>(job);
    }
}
