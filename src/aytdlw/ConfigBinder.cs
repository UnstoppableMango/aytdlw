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
        return new(_configDir, bindingContext.Console);
    }
}
