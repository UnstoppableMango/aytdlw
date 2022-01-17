namespace Aytdlw.Service.Models;

public static class AytdlwOptionsExtensions
{
    public static IEnumerable<string> GetProcessArguments(this AytdlwOptions options)
    {
        var arguments = new List<string>();

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

        return arguments;
    }
}
