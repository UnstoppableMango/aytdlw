using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public class YtdlpProcess : IYoutubeDl
{

    public ValueTask<DownloadJob> Download(string url, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
