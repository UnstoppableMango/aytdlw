using Aytdlw.Service.Models;
using Aytdlw.Service.Services;

var builder = WebApplication.CreateBuilder(args);

var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
if (!string.IsNullOrWhiteSpace(xdgConfigHome)) {
    var configFile = Path.Combine(xdgConfigHome, "config.json");
    builder.Configuration.AddJsonFile(configFile, false, true);
}

var home = Environment.GetEnvironmentVariable("HOME");
if (!string.IsNullOrWhiteSpace(home)) {
    var configFile = Path.Combine(home, ".config", "aytdlw", "config.json");
    builder.Configuration.AddJsonFile(configFile, false, true);
}

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<ITaskQueue, BackgroundTaskQueue>();
builder.Services.AddTransient<IYoutubeDl, YtdlpProcess>();
builder.Services.AddTransient<IJobReporter, ConsoleJobReporter>();
builder.Services.AddHostedService<QueueProcessor>();
builder.Services.Configure<AytdlwOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DownloadQueueService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
