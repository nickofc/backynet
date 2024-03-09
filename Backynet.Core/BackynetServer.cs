using Backynet.Core.Abstraction;

namespace Backynet.Core;

internal sealed class BackynetServer : IBackynetServer
{
    private readonly IThreadPool _threadPool;
    private readonly BackynetServerOptions _backynetServerOptions;
    private readonly IStorageListener<object> _storageListener;
    private readonly IDistributedLock _distributedLock;
    private readonly IStorage _storage;

    public async Task Start(CancellationToken cancellationToken)
    {
        await _storageListener.Start(Callback, cancellationToken);

        
        // event pojawia się po dodaniu joba do tabeli 
        // wykonaj select (where serverId = null) & update (serverId = currentServerId) do jakiego serwera jest przypisany 
    }

    private async Task Callback(object arg)
    {
        var jobId = arg as string;
        
        if (await _distributedLock.TryAcquire(jobId))
        {
            var guidJobId = Guid.Parse(jobId);
            
            var job = await _storage.Get(guidJobId);
        }
    }
}