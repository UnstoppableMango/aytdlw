using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public interface IYoutubeDl
{
    ValueTask<DownloadJob> DownloadAsync(string url, CancellationToken cancellationToken = default);
}
