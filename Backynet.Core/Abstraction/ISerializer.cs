namespace Backynet.Core.Abstraction;

public interface ISerializer
{
    string Serialize<T>(T instance);
    T Deserialize<T>(string payload);
}