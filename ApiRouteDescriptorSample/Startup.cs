using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRouteDescriptor;
using ApiRouteDescriptor.EntityFrameworkCore.Extensions;
using ApiRouteDescriptor.Extensions;
using ApiRouteDescriptor.Responders;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace ApiRouteDescriptorSample
{
    public class Startup
    {
        public Startup()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Person, PersonResource>();
            });
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var conneciton = @"Server=(localdb)\mssqllocaldb;Database=ApiRouteDescriptorSample;Trusted_Connection=True;";
            services.AddApiRouteDescriptor().AddEntityFrameworkDataStore<SampleContext>(o => o.UseSqlServer(conneciton));

            //services.AddApiRouteDescriptor().AddInMemoryDataStore();
            services.AddScoped<HomeResponder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseApiRouteDescriptor(new SampleApiDefinition());
        }

        public class SampleApiDefinition : ApiDefinition
        {
            public SampleApiDefinition()
            {
                this.Get["Root", "api"] = this.CustomAction<HomeResponder>();
                this.Get["People", "api/people"] = this.Paged<Person, PersonResource>("Name");
            }
        }

        public class HomeResponder : CustomActionResponder<HomeResponder>
        {
            protected override Task Execute()
            {
                return this.Context.Response.WriteAsync("Hello");
            }
        }

        public class SampleContext : DbContext
        {
            public SampleContext(DbContextOptions options) : base(options)
            {
            }

            public DbSet<Person> Persons { get; set; }
        }

        public class Person
        {
            public string Id { get; set; }
            public string Name { get; set; }

        }

        public class PersonResource
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

    }

}
