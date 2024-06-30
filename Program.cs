var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls("http://*:80");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
var defaultPath = Environment.GetEnvironmentVariable("ROOT_PATH");
var destination = Environment.GetEnvironmentVariable("DESTINATION");
var appName = Environment.GetEnvironmentVariable("APP_NAME");

app.MapGet($"{defaultPath}/", () => $"Hello World! for destination {destination} from {appName} app\n");
app.MapGet($"{defaultPath}/simulate/status/200", () => $"Successfull request for destination {destination} from {appName} app\n");

app.Run();
