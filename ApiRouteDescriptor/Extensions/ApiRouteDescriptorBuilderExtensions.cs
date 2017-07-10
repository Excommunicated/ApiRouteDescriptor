using ApiRouteDescriptor.Data;
using ApiRouteDescriptor.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRouteDescriptor.Extensions
{
    public static class ApiRouteDescriptorBuilderExtensions
    {
        public static ApiRouteDescriptorBuilder UseInMemoryDataStore(this ApiRouteDescriptorBuilder builder)
        {
            builder.Services.AddSingleton<IDataStore, InMemoryDataStore>();
            return builder;
        }

        public static ApiRouteDescriptorBuilder UseResourceMapper<TResourceMapper>(
            this ApiRouteDescriptorBuilder builder) where TResourceMapper : class, IResourceMapper
        {
            builder.Services.AddSingleton<IResourceMapper, TResourceMapper>();
            return builder;
        }
    }
}