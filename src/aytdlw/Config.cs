using System.CommandLine;
using System.Text.Json;

namespace aytdlw;

public interface IConfig
{
    string Get(string key);
    void Set(string key, string value);
}

public class Config : IConfig
{
    private readonly IConsole? _console;
    private readonly string _configFile;
    
    public Config(string configDir, IConsole? console)
    {
        _console = console;
        _configFile = Path.Combine(configDir, "config.json");
    }
    
    public string Get(string key)
    {
        return "TODO";
    }
    
    public void Set(string key, string value)
    {
        EnsureInitialized();
        
        var contents = File.OpenRead(_configFile);
        var config = JsonSerializer.Deserialize<AytdlwConfig>(contents);

        if (config == null) {
            _console.WriteLine("Unable to deserialize config");
        }
    }

    private void EnsureInitialized()
    {
        if (!File.Exists(_configFile)) {
            File.WriteAllText(_configFile, "{}");
        }
    }
}
