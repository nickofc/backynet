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
        
    }

    private async Task Callback(object arg)
    {
        var jobId = Guid.Parse((string)arg);

/* TODO:

Co trzeba dodać do jobów? 

    1. cron i planowanie zadan 
    2. grupy i konfiguracja ile workerów w tym samym czasie może pracować dla jednej grupy


Jak rozwiązać? 

    1. moze byc sytuacja ze workery sa w uzyciu i nie będzie mozna wykonac danego joba a inny serwer moze bedzie wolny
    2. jak rozwiazac problem z jobami ktore byly wykonywane na jakims serwerze ktory zostal wylaczony? 
    3. jak zaplanować joby?
    4. jak zaplanowac joby rekurenyjne?
    5. jak ograniczyć workery dla grupy?
    
    ofc z naciskiem na wydajność 

*/
        
        // sprawdź czy mozna wykonac od razu
        // jezeli nie dodaj do kolejki i sprawdz gdy jakis worker/serwer będzie wolny
        
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