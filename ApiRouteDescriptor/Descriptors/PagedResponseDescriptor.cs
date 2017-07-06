using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using ApiRouteDescriptor.Resources;
using ApiRouteDescriptor.Responders;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiRouteDescriptor.Descriptors
{
    public class PagedResponseDescriptor<TModel, TResource>: ModelResponseDescriptor where TModel : class
    {

        public int ItemsPerPage { get; set; } = 30;
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }

        public PagedResponseDescriptor(string orderBy, bool orderDesc) : base(typeof(TModel), typeof(PagedResponder))
        {
            OrderBy = orderBy;
            OrderDesc = orderDesc;
            this.ResourceTypes.Add(typeof(TResource));
        }


        public class PagedResponder : Responder<PagedResponseDescriptor<TModel, TResource>> 
        {
            private readonly IDataStore _dataStore;

            public PagedResponder(IDataStore dataStore)
            {
                _dataStore = dataStore;
            }
            protected override Task Execute()
            {
                var skip = this.QueryString("skip", 0);
                this.Options.ItemsPerPage = this.QueryString("take", 30);
                var query = this._dataStore.Query<TModel>();
                var orderedResults = query.OrderBy(this.Options.OrderBy +
                                                   (this.Options.OrderDesc ? " descending": " ascending"));
                int totalResults = orderedResults.Count();
                var results = orderedResults.Skip(skip).Take(this.Options.ItemsPerPage).ToList();
                var collection = new ResourceCollection<TResource>
                {
                    ItemsPerPage = this.Options.ItemsPerPage,
                    CurrentIndex = skip,
                    TotalResults = totalResults,
                    Items = results.Select(AutoMapper.Mapper.Map<TResource>).ToList()
                };
                this.Context.Response.ContentType = "application/vnd.apiroutedescriptor+json";
                return this.Context.Response.WriteAsync(JsonConvert.SerializeObject(collection));
            }
        }
    }
}
