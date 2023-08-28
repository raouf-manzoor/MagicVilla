using MagicVilla_API.Repository.IRepository;
using System.Reflection;

namespace MagicVilla_API.Extentions
{
    public static class AutomaticDependencyRegistrar
    {

        // In test mode code for registering dependencies automatically

        public static IServiceCollection AddServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddServicesFromAssembly(assembly, typeof(IVillaRepository), ServiceLifetime.Scoped);
            services.AddServicesFromAssembly(assembly, typeof(IVillaNumberRepository), ServiceLifetime.Scoped);
            services.AddServicesFromAssembly(assembly, typeof(IUserRepository), ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddServicesFromAssembly(this IServiceCollection services,
            Assembly assembly,
            Type markerInterface,
            ServiceLifetime lifeTime
            )
        {

            var condidateTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && ImplementInterface(t, markerInterface)
            );

            // To Inject the concrete implementation

            foreach (var concreteImplementation in condidateTypes)
            {

                var serviceInterface = concreteImplementation.GetInterfaces().FirstOrDefault(e => e != markerInterface);

                if (serviceInterface is not null)
                {
                    services.AddService(serviceInterface, concreteImplementation, ServiceLifetime.Scoped);
                }
            }

            return services;
        }

        public static bool ImplementInterface(Type type, Type makerInterface)
        {
            return makerInterface.IsAssignableFrom(type);
            //|| Array.Exists(type.GetInterfaces(),t=>t==makerInterface.IsAssignableFrom);
            return true;
        }

        public static IServiceCollection AddService(this IServiceCollection services,
            Type serviceInterface,
            Type concreteImplementation,
            ServiceLifetime lifetime)
        {
            if (lifetime == ServiceLifetime.Transient)
            {
                services.AddTransient(serviceInterface, concreteImplementation);
            }
            else if (lifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(serviceInterface, concreteImplementation);
            }
            else if (lifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(serviceInterface, concreteImplementation);
            }

            return services;
        }
    }
}
