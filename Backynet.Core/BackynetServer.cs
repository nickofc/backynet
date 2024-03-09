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

        // trzeba zrobic jakas kolejke? 
        // moze byc sytuacja ze workery sa w uzyciu i nie będzie mozna wykonac danego joba a inny serwer moze bedzie wolny
    }

    private async Task Callback(object arg)
    {
        var jobId = Guid.Parse((string)arg);

        // sprawdź czy mozna wykonac od razu
        // jezeli nie dodaj do kolejki i sprawdz gdy jakis worker/serwer będzie wolny

        // jak rozwiazac problem z jobami ktore byly wykonywane na jakims serwerze ktory zostal wylaczony? 

        if (await IsWorkerAvailable())
        {
            if (await TryAcquire(jobId))
            {
                
            }
        }
        else
        {
        }
    }

    private async Task<bool> TryAcquire(Guid jobId)
    {
        return true;
    }

    private async Task<bool> IsWorkerAvailable()
    {
        return false;
    }
}