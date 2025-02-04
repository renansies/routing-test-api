using System.Text.Json;
using Vtex.Observability.Utilities.UserImplementedInterfaces;

namespace RoutingTestAPI.Observability;

public class ObservabilityJsonSerializer : IVtexObservabilityLibraryJsonSerializer
{
    public T? CaseInsensitiveDeserialize<T>(string str)
    {
        return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
    }
}