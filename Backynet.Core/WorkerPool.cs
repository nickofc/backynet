using System.Threading.Channels;

namespace Backynet.Core;

internal sealed class WorkerPool<T>
{
    public int TaskCount { get; }
    public Func<T, CancellationToken, Task> Callback { get; }

    public WorkerPool(int taskCount, Func<T, CancellationToken, Task> callback)
    {
        TaskCount = taskCount;
        Callback = callback;
    }

    private CancellationTokenSource _cts;
    private Task _mainTask;
    private Channel<T> _channel;

    public Task Start(CancellationToken cancellationToken)
    {
        var tasks = new Task[TaskCount];

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _channel = Channel.CreateUnbounded<T>();

        for (var i = 0; i < TaskCount; i++)
        {
            var worker = new Worker<T>(_channel.Reader, Callback);
            tasks[i] = worker.Start(_cts.Token);
        }

        _mainTask = Task.WhenAll(tasks);
        return _mainTask.IsCompleted ? _mainTask : Task.CompletedTask;
    }

    public async Task Post(T data)
    {
        await _channel.Writer.WriteAsync(data);
    }
}