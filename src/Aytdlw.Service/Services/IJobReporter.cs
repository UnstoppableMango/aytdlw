using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public interface IJobReporter
{
    void Watch(DownloadJob job);
}
