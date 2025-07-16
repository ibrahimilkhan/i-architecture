using System;
using System.Reflection;
using Core.Application.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        return services;
    }

    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services, 
        Assembly assembly, 
        Type type, 
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(x=>x.IsSubclassOf(type) && type != x).ToList();

        foreach (var t in types)
        {
            if (addWithLifeCycle != null)
            {
                addWithLifeCycle(services, t);
            }
            else
            {
                services.AddScoped(t);
            }
        }

        return services;
    }
}