using System.Threading.Channels;

namespace Backynet.Core;

public class Worker
{
    private readonly ChannelReader<Data> _channelReader;

    public Worker(ChannelReader<Data> channelReader)
    {
        _channelReader = channelReader;
    }

    public async Task Start(CancellationToken cancellationToken = default)
    {
    }
}