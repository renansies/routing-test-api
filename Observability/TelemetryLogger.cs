using Vtex.Diagnostics;
using Vtex.Observability.Utilities.UserImplementedInterfaces;

namespace RoutingTestAPI.Observability;

public class TelemetryLogger: IVtexObservabilityLibraryLogger
{
    public void Info(string logKey, Dictionary<string, string> tags)
    {
        Log.Info("Tracing",
            fields: Fields(tags)
        );
    }

    public void Error(string logKey, Dictionary<string, string> tags, Exception ex)
    {

        Log.Error("Tracing",
            fields: Fields(tags),
            exception: ex
        );
    }

    private Field[] Fields(Dictionary<string, string> tags)
    {
        var fields = new Field[tags.Count];
        var i = 0;
        foreach (var entry in tags)
        {
            fields[i] = new Field(entry.Key, entry.Value);
            i++;
        }

        return fields;
    }
}

    
