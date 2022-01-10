using System.CommandLine;
using System.CommandLine.Binding;

namespace aytdlw;

public class ConfigBinder : BinderBase<Config>
{
    private readonly string _configDir;

    public ConfigBinder(string configDir)
    {
        _configDir = configDir;
    }
    
    protected override Config GetBoundValue(BindingContext bindingContext)
    {
        var console = bindingContext.GetService(typeof(IConsole));
        return new(_configDir, console as IConsole);
    }
}
