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

    public BackynetServerOptions()
    {
        MaxThreads = Environment.ProcessorCount;
    }
}