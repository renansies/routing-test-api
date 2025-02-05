using System.Diagnostics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddEndpointsApiExplorer()
    .AddHttpClient();

var defaultPath = Environment.GetEnvironmentVariable("ROOT_PATH");
var destination = Environment.GetEnvironmentVariable("DESTINATION");
var appName = Environment.GetEnvironmentVariable("APP_NAME");
var routeApp = Environment.GetEnvironmentVariable("ROUTE_APP");
var cell = Environment.GetEnvironmentVariable("CELL");

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService($"{appName}-app"))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService($"{appName}-app"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri("http://localhost:5000");
        })
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());


builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Activity.Current?.SetTag("app", appName);
Activity.Current?.SetTag("version", "v1.0.0-test");
Activity.Current?.SetTag("stack", cell);

Activity.Current?.SetBaggage("tenant", "foo-account");

app.MapGet($"{defaultPath}/hello-world", () => $"Hello World from {appName} app in cell {cell}!\n");
app.MapGet($"{defaultPath}/simulate/status/200", () => $"Successfull request for destination {destination} from {appName} app in cell {cell}\n");

app.MapGet($"{defaultPath}/simulate/route/{routeApp}", async (HttpClient httpClient) =>
{
    var redirectUrl = $"http://host.containers.internal/api/{routeApp}/simulate/status/200";

    try
    {
        var response = await httpClient.GetAsync(redirectUrl);

        if (!response.IsSuccessStatusCode)
            return Results.Problem(
                $"Error to route from {routeApp} app in cell {cell}: {response.StatusCode} - {response.ReasonPhrase}\n!");
        var data = await response.Content.ReadAsStringAsync();
        return Results.Ok(data);

    }
    catch (Exception ex)
    {
        return Results.Problem($"Error: {ex.Message}\n{ex.StackTrace}");
    }
});

app.Run();
