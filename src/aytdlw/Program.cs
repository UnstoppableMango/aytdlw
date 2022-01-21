using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;

using Aytdlw;
using Aytdlw.Service;

using Microsoft.Extensions.DependencyInjection;

var configBase = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
if (string.IsNullOrWhiteSpace(configBase)) {
    var homeDir = Environment.GetEnvironmentVariable("HOME");
    if (!string.IsNullOrWhiteSpace(homeDir))
        configBase = Path.Combine(homeDir, ".config");
}

if (string.IsNullOrWhiteSpace(configBase)) {
    Console.WriteLine("Unable to resolve config dir");
    return 1;
}

var configDir = Path.Combine(configBase, "aytdlw");

if (!Directory.Exists(configDir)) {
    Console.WriteLine("Creating config directory");
    Directory.CreateDirectory(configDir);
}

var serviceCollection = new ServiceCollection()
    .AddTransient<IConfig>(sp => new Config(configDir, sp.GetRequiredService<IConsole>()))
    .AddTransient<IConsole, SystemConsole>();

serviceCollection.AddGrpcClient<DownloadQueue.DownloadQueueClient>(options => {
    options.Address = new Uri("http://localhost:5190");
});

var services = serviceCollection.BuildServiceProvider();

var configBinder = new ServiceProviderBinder<IConfig>(services);
var clientBinder = new ServiceProviderBinder<DownloadQueue.DownloadQueueClient>(services);

var configGetKeyArgument = new Argument<string>("key", "The key of the configuration value to get");
var configGetCommand = new Command("get", "Get a configuration value") {
    configGetKeyArgument
};
configGetCommand.SetHandler(
    (string key, IConfig config, IConsole console) => {
        console.WriteLine(config.Get(key) ?? string.Empty);
    },
    configGetKeyArgument,
    configBinder);

var configRemoveKeyArgument = new Argument<string>("key", "The configuration key to remove");
var configRemoveCommand = new Command("remove", "Remove a configuration value") {
    configRemoveKeyArgument
};
configRemoveCommand.AddAlias("rm");
configRemoveCommand.SetHandler(
    (string key, IConfig config) => {
        config.Remove(key);
    },
    configRemoveKeyArgument,
    configBinder);

var configSetKeyArgument = new Argument<string>("key", "The key of the configuration value to set");
var configSetValueArgument = new Argument<string>("value", "The value to set");
var configSetCommand = new Command("set", "Set a configuration value") {
    configSetKeyArgument,
    configSetValueArgument
};
configSetCommand.SetHandler(
    (string key, string value, IConfig config) => {
        config.Set(key, value);
    },
    configSetKeyArgument,
    configSetValueArgument,
    configBinder);

var configCommand = new Command("config", "Get and set configuration") {
    configGetCommand,
    configRemoveCommand,
    configSetCommand
};

var queueUrlArgument = new Argument<string>("url", "The url for the download to queue");
var queueCommand = new Command("queue", "Queue a download") {
    queueUrlArgument,
};
queueCommand.AddAlias("q");
queueCommand.SetHandler(
    (string url, DownloadQueue.DownloadQueueClient client, IConsole console, CancellationToken cancellationToken) => {
        var reply = client.Enqueue(new() {
            Url = url
        }, cancellationToken: cancellationToken);
        
        console.WriteLine($"{reply.Id}");
        console.WriteLine(reply.Message);
    },
    queueUrlArgument,
    clientBinder);

var startCommand = new Command("start", "Start the background service");
startCommand.SetHandler((IConsole console, IConfig config) => {
    console.WriteLine("TODO");
}, configBinder);

var statusCommand = new Command("status", "Get the status of the background service");
var stopCommand = new Command("stop", "Stop the background service");

var command = new RootCommand {
    configCommand,
    queueCommand,
    startCommand,
    statusCommand,
    stopCommand,
};

var parser = new CommandLineBuilder(command)
    .UseDefaults()
    .Build();

return parser.Invoke(args);
