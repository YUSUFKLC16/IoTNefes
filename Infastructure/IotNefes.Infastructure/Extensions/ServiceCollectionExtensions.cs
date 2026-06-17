using System.Reflection;
using IotNefes.Infastructure.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace IotNefes.Infastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesByConvention(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t is { IsClass: true, IsAbstract: false });

                foreach (var type in types)
                {
                    if (typeof(IScopedService).IsAssignableFrom(type))
                        RegisterService(services, type, ServiceLifetime.Scoped);
                    else if (typeof(ITransientService).IsAssignableFrom(type))
                        RegisterService(services, type, ServiceLifetime.Transient);
                    else if (typeof(ISingletonService).IsAssignableFrom(type))
                        RegisterService(services, type, ServiceLifetime.Singleton);
                }
            }

            return services;
        }

        private static void RegisterService(IServiceCollection services, Type implementationType, ServiceLifetime lifetime)
        {
            var interfaces = implementationType.GetInterfaces()
                .Where(i => i != typeof(IScopedService)
                         && i != typeof(ITransientService)
                         && i != typeof(ISingletonService));

            foreach (var @interface in interfaces)
            {
                services.Add(new ServiceDescriptor(@interface, implementationType, lifetime));
            }
        }
    }
}
