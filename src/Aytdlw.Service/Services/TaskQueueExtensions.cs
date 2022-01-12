using Aytdlw.Service.Models;

namespace Aytdlw.Service.Services;

public static class TaskQueueExtensions
{
    public static ValueTask<int> QueueUrlDownload(
        this ITaskQueue queue,
        EnqueueRequest request,
        CancellationToken cancellationToken = default)
    {
        // TODO: Probably add state param to avoid capturing request in closure
        return queue.EnqueueAsync((s, c) => Work(s, request, c), cancellationToken);
    }

    private static async ValueTask Work(
        IServiceProvider services,
        EnqueueRequest request,
        CancellationToken cancellationToken)
    {
        var ytdl = services.GetRequiredService<IYoutubeDl>();
        var reporter = services.GetRequiredService<IJobReporter>();
        var job = await ytdl.DownloadAsync(request.Url, cancellationToken);
        reporter.Watch(job);
    }
}
