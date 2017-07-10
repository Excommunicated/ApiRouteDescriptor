using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using ApiRouteDescriptor.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ApiRouteDescriptor.EntityFrameworkCore.Extensions
{
    public static class ApiRouteDescriptorBuilderExtensions
    {
        public static ApiRouteDescriptorBuilder UseEntityFrameworkDataStore<TContext>(this ApiRouteDescriptorBuilder builder, Action<DbContextOptionsBuilder> options) where TContext: DbContext
        {
            builder.Services.AddDbContext<TContext>(options);
            builder.Services.AddScoped<IDataStore, EntityFrameworkDataStore<TContext>>();
            return builder;
        }
    }
}
