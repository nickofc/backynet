using System.Threading.Channels;

namespace Backynet.Core;

public class WorkerPool
{
    public int TaskCount { get; }

    public WorkerPool(int taskCount)
    {
        TaskCount = taskCount;
    }

    public async Task Start()
    {
        var tasks = new List<Task>(TaskCount);

        // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl 
        // doczytać czy tpl/channel?
    }
}