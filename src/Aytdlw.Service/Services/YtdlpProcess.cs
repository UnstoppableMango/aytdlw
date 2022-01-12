using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public class YtdlpProcess : IYoutubeDl
{

    public ValueTask<DownloadJob> DownloadAsync(string url, CancellationToken cancellationToken = default)
    {
        return new ValueTask<DownloadJob>(new DownloadJob());
    }
}
