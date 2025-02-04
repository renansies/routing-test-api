using System.Text.Json;
using Vtex.Observability.Utilities.UserImplementedInterfaces;

namespace RoutingTestAPI.Observability;

public class FetchConfig : IVtexObservabilityLibraryS3Getter
{

    public Task<string> GetObjectAsString(string bucket, string path, CancellationToken cancellationToken)
    {
        var config = JsonSerializer.Serialize("{\n  \"SampledLogsExtension\": {\n    \"PerKpiKeySamplingConfiguration\": {\n      \"RequestData\": {\n        \"DefaultSampleRate\": 1,\n        \"PerSamplingKeySamplingRate\": {}\n      }\n    },\n    \"DisabledKpis\": []\n  },\n  \"OpenTelemetryConfig\": {\n    \"TracingConfig\": {\n      \"EnableTracing\": true,\n      \"EnableDebugMode\": true,\n      \"Sampling\": {\n        \"OverrideIncomingTraceRecordedDecision\": false,\n        \"OverrideIncomingTraceDiscardedDecision\": false,\n        \"PerAccountConfig\": {\n          \"*\": {\n            \"DefaultSamplingRate\": 1,\n            \"PerOperationSamplingRate\": {}\n          }\n        }\n      },\n      \"ServerHeadersTracking\": {\n        \"RequestHeaders\": [],\n        \"ResponseHeaders\": []\n      },\n      \"ClientHeadersTracking\": {\n        \"RequestHeaders\": [],\n        \"ResponseHeaders\": []\n      },\n      \"ActivityTracerConfig\": {\n        \"TraceDecisionPropagationConfig\": {\n          \"EnsureEveryIncomingTraceIsFromVtex\": false,\n          \"AccountsToEnsureIncomingTraceIsFromVtex\": []\n        }\n      }\n    }\n  }\n}");
        return Task.FromResult(config);
    }
}