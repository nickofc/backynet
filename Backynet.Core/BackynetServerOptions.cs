namespace Backynet.Core;

internal sealed class BackynetServerOptions
{
    private int _maxThreads;

    public int MaxThreads
    {
        get => _maxThreads;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
            _maxThreads = value;
        }
    }

    private string _serverName;

    public string ServerName
    {
        get => _serverName;
        set
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            _serverName = value;
        }
    }

    public BackynetServerOptions()
    {
        MaxThreads = Environment.ProcessorCount;
        ServerName = Environment.MachineName;
    }
}