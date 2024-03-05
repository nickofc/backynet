using System.Threading.Channels;

namespace Backynet.Core;

public sealed class Worker<T>
{
    private readonly ChannelReader<T> _channelReader;
    private readonly Func<T, CancellationToken, Task> _function;

    public Worker(ChannelReader<T> channelReader, Func<T, CancellationToken, Task> function)
    {
        _channelReader = channelReader;
        _function = function;
    }

    private Task? _mainTask;
    private CancellationTokenSource? _cts;

    public Task Start(CancellationToken cancellationToken = default)
    {
        if (_mainTask != null)
        {
            return Task.CompletedTask;
        }

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _mainTask = StartCore(_cts.Token);

        return _mainTask.IsCompleted ? _mainTask : Task.CompletedTask;
    }

    private async Task StartCore(CancellationToken cancellationToken)
    {
        while (await _channelReader.WaitToReadAsync(cancellationToken))
        {
            if (!_channelReader.TryRead(out var item))
            {
                continue;
            }

            await _function(item, cancellationToken);
        }
    }

    public async Task Stop(CancellationToken cancellationToken = default)
    {
        if (_mainTask == null)
        {
            return;
        }

        await _cts!.CancelAsync();

        try
        {
            await _mainTask.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException e)
        {
            if (e.CancellationToken != _cts.Token)
            {
                throw;
            }
        }
    }
}