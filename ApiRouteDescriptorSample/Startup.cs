using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiRouteDescriptor;
using ApiRouteDescriptor.EntityFrameworkCore.Extensions;
using ApiRouteDescriptor.Extensions;
using ApiRouteDescriptor.Resources;
using ApiRouteDescriptor.Responders;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            var connection = @"Server=(localdb)\mssqllocaldb;Database=ApiRouteDescriptorSample;Trusted_Connection=True;";
            services.AddApiRouteDescriptor(config =>
            {
                config
                    .UseEntityFrameworkDataStore<SampleContext>(o => o.UseSqlServer(connection))
                    .UseResourceMapper<SampleResourceMapper>();
            });

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
                this.Get["People", "api/people"] = this.Paged<Person, PersonResource>("Name").WithLinkRouteName("People");
                this.Get["Person", "api/people/{id}"] = this.Single<Person,PersonResource,string>();
            }
        }

        public class HomeResponder : CustomActionResponder<HomeResponder>
        {
            protected override async Task<Resource> Execute()
            {
                return new HomeResource
                {
                    ApplicationName = "SampleApiApplication",
                    Version = "1.0.0.0",
                    Links = LinkCollection.Self(this.ResolveLink("Root"))
                                .Add("People",this.ResolveLink("People"))
                };
            }

            public class HomeResource : Resource
            {
                public string ApplicationName { get; set; }
                public string Version { get; set; }
            }
        }
        
        public class SampleResourceMapper : ResourceMapper
        {
            public override void InitializeMappings()
            {
                this.Map<Person, PersonResource>().EnrichResource((person, resource, helper) =>
                {
                    resource.Links = LinkCollection.Self(helper.ResolveLink("Person",new {resource.Id}));
                });
            }

            public SampleResourceMapper(IUrlHelper helper) : base(helper)
            {
            }
        }

        public class SampleContext : DbContext
        {
            public SampleContext(DbContextOptions options) : base(options)
            {
            }

            public DbSet<Person> Persons { get; set; }
        }

        public class Person : IHaveId<string>
        {
            public string Id { get; set; }
            public string Name { get; set; }

        }

        public class PersonResource : Resource
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

    }

}
