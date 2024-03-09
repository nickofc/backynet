using System.Threading.Channels;

namespace Backynet.Core;

internal sealed class Worker<T>
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

        _mainTask.ContinueWith(_ => ReleaseResources(), cancellationToken);

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
            if (e.CancellationToken == cancellationToken)
            {
                throw;
            }
        }
    }

    private void ReleaseResources()
    {
        if (_mainTask != null)
        {
            _mainTask.Dispose();
            _mainTask = null;
        }

        if (_cts != null)
        {
            _cts.Dispose();
            _cts = null;
        }
    }
}