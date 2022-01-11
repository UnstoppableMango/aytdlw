using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public interface IYoutubeDl
{
    ValueTask<DownloadJob> Download(string url, CancellationToken cancellationToken = default);
}
