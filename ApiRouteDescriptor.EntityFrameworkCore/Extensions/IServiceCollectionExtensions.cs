using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRouteDescriptor.EntityFrameworkCore.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkDataStore<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TContext: DbContext
        {
            services.AddDbContext<TContext>(options);
            services.AddScoped<IDataStore, EntityFrameworkDataStore<TContext>>();
            return services;
        }
    }
}
