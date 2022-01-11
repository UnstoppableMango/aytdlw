using System.CommandLine;
using System.Text.Json;

namespace aytdlw;

public interface IConfig
{
    string? Get(string key);

    void Remove(string key);
    
    void Set(string key, string value);
}

public class Config : IConfig
{
    private readonly IConsole _console;
    private readonly string _configFile;
    
    public Config(string configDir, IConsole console)
    {
        _console = console;
        _configFile = Path.Combine(configDir, "config.json");
    }
    
    public string? Get(string key)
    {
        EnsureInitialized();
        
        var contents = File.OpenRead(_configFile);
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(contents);

        if (config == null) {
            throw new("Unable to deserialize config");
        }

        return config.TryGetValue(key, out var result) ? result : null;
    }

    public void Remove(string key)
    {
        EnsureInitialized();
        
        var contents = File.OpenRead(_configFile);
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(contents);

        if (config == null) {
            throw new("Unable to deserialize config");
        }

        config.Remove(key);

        var json = JsonSerializer.Serialize(config);
        File.WriteAllText(_configFile, json);
    }
    
    public void Set(string key, string value)
    {
        EnsureInitialized();
        
        var contents = File.OpenRead(_configFile);
        var config = JsonSerializer.Deserialize<Dictionary<string, string>>(contents);

        if (config == null) {
            throw new("Unable to deserialize config");
        }

        config[key] = value;

        var json = JsonSerializer.Serialize(config);
        File.WriteAllText(_configFile, json);
    }

    private void EnsureInitialized()
    {
        if (File.Exists(_configFile)) {
            return;
        }

        _console.WriteLine($"Initializing config at {_configFile}");
        var json = JsonSerializer.Serialize(new Dictionary<string, string>());
        File.WriteAllText(_configFile, $"{json}\n");
    }
}
