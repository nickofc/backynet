using Backynet.Core.Abstraction;

namespace Backynet.Core;

public class BackynetServer : IBackynetServer
{
    private readonly IThreadPool _threadPool;
    private readonly BackynetServerOptions _backynetServerOptions;
    private readonly IStorageListener _storageListener;
    private readonly IDistributedLock _distributedLock;
    private readonly IStorage _storage;

    public BackynetServer(IThreadPool threadPool, BackynetServerOptions backynetServerOptions)
    {
        _threadPool = threadPool;
        _backynetServerOptions = backynetServerOptions;
    }

    public Task Start(CancellationToken cancellationToken)
    {
        _storageListener.OnItemAdded += StorageListenerOnOnItemAdded;

        // huh? jak to wykonać? 
        async void StorageListenerOnOnItemAdded(object? sender, string jobId)
        {
            // event pojawia się po dodaniu joba do tabeli 
            // wykonaj select (where serverId = null) & update (serverId = currentServerId) do jakiego serwera jest przypisany 
            
            await Task.Run(async () =>
            {
                if (await _distributedLock.TryAcquire(jobId))
                {
                    var guidJobId = Guid.Parse(jobId);
                    var job = await _storage.Get(guidJobId, cancellationToken);
                    
                }
                
                
            }, CancellationToken.None);
        }

        return _storageListener.Start(cancellationToken);
    }
}