using Aytdlw.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<ITaskQueue, BackgroundTaskQueue>();
builder.Services.AddTransient<IYoutubeDl, YtdlpProcess>();
builder.Services.AddHostedService<QueueProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DownloadQueueService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
