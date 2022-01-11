using Grpc.Core;

namespace Aytdlw.Service.Services;

public class DownloadQueueService : DownloadQueue.DownloadQueueBase
{
    private readonly ITaskQueue _queue;

    public DownloadQueueService(ITaskQueue queue)
    {
        _queue = queue;
    }
    
    public override async Task<EnqueueReply> Enqueue(EnqueueRequest request, ServerCallContext context)
    {
        // TODO: Probably add state param to avoid capturing request in closure
        var id = await _queue.EnqueueAsync(async (services, cancellationToken) => {
            var youtubeDl = services.GetRequiredService<IYoutubeDl>();
            return await youtubeDl.Download(request.Url, cancellationToken);
        }, context.CancellationToken);
        
        return new EnqueueReply {
            Id = id,
            Message = "Job enqueued"
        };
    }
}
