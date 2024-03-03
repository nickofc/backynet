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
        return JsonConvert.DeserializeObject<T>(payload, DefaultJsonSerializerSettings);
    }
}