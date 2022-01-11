using System.CommandLine.Binding;

namespace Aytdlw;

public class ServiceProviderBinder<T> : BinderBase<T>
    where T : class
{
    private readonly IServiceProvider _services;

    public ServiceProviderBinder(IServiceProvider services)
    {
        _services = services;
    }

    protected override T GetBoundValue(BindingContext bindingContext)
    {
        var service = _services.GetService(typeof(T));
        if (service is null) {
            throw new InvalidOperationException($"Unable to resolve service for type {typeof(T)}");
        }
        
        return (T)service;
    }
}
