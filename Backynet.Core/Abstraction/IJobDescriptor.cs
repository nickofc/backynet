namespace Backynet.Core.Abstraction;

public interface IJobDescriptor
{
    string BaseType { get; }
    string Method { get; }
    object[] Arguments { get; }
}