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
        var id = await _queue.QueueUrlDownload(request, context.CancellationToken);
        
        return new EnqueueReply {
            Id = id,
            Message = "Job enqueued"
        };
    }
}
