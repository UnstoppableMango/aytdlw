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

var queueCommand = new Command("queue", "Queue a download");

var startCommand = new Command("start", "Start the background service");
startCommand.SetHandler((IConsole console) => {
    // TODO
}, new ConfigBinder(configDir));

var statusCommand = new Command("status", "Get the status of the background service");
var stopCommand = new Command("stop", "Stop the background service");

var command = new RootCommand {
    queueCommand,
    startCommand,
    statusCommand,
    stopCommand,
};

var parser = new CommandLineBuilder(command)
    .UseDefaults()
    .Build();

return parser.Invoke(args);
