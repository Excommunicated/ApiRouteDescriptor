using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiRouteDescriptor.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiRouteDescriptor(this IServiceCollection services)
        {
            services.AddRouting();
            var types = typeof(ApiDefinition).GetTypeInfo().Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i=>i.Name.StartsWith("IResponder")) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in types)
            {
                services.TryAddScoped(type);
            }

            return services;
        }

        public static IServiceCollection AddInMemoryDataStore(this IServiceCollection services)
        {
            services.AddSingleton<IDataStore, InMemoryDataStore>();
            return services;
        }
    }
}
