using Backynet.Core.Abstraction;
using Newtonsoft.Json;

namespace Backynet.Core;

public class DefaultJsonSerializer : ISerializer
{
    private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto
    };

    public string Serialize<T>(T instance)
    {
        return JsonConvert.SerializeObject(instance, DefaultJsonSerializerSettings);
    }

    public T Deserialize<T>(string payload)
    {
        var obj = JsonConvert.DeserializeObject<T>(payload, DefaultJsonSerializerSettings);
        return obj == null ? throw new InvalidOperationException() : obj;
    }
}