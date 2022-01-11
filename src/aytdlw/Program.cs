using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using aytdlw;

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

var configBinder = new ConfigBinder(configDir);

var configGetKeyArgument = new Argument<string>("key", "The key of the configuration value to get");
var configGetCommand = new Command("get", "Get a configuration value") {
    configGetKeyArgument
};
configGetCommand.SetHandler((string key, IConfig config, IConsole console) => {
    console.WriteLine(config.Get(key) ?? string.Empty);
},
    configGetKeyArgument,
    configBinder);

var configRemoveKeyArgument = new Argument<string>("key", "The configuration key to remove");
var configRemoveCommand = new Command("remove", "Remove a configuration value") {
    configRemoveKeyArgument
};
configRemoveCommand.AddAlias("rm");
configRemoveCommand.SetHandler((string key, IConfig config) => {
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
configSetCommand.SetHandler((string key, string value, IConfig config) => {
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

var queueCommand = new Command("queue", "Queue a download");

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
