using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using ApiRouteDescriptor.Extensions;
using ApiRouteDescriptor.Resources;
using ApiRouteDescriptor.Responders;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiRouteDescriptor.Descriptors
{
    public class PagedResponseDescriptor<TModel, TResource>: ModelResponseDescriptor, IPage where TModel : class where TResource: Resource
    {

        public int ItemsPerPage { get; set; } = 30;
        public string LinkRouteName { get; set; }
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
            private readonly IResourceMapper _mapper;
            private readonly IUrlHelper _helper;

            public PagedResponder(IDataStore dataStore, IResourceMapper mapper, IUrlHelper helper)
            {
                _dataStore = dataStore;
                _mapper = mapper;
                _helper = helper;
            }
            protected override async Task<Resource> Execute()
            {
                var skip = this.QueryString("skip", 0);
                var linkRouteName = this.Options.LinkRouteName;
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
                    Items = results.Select(x=> _mapper.MapTo<TModel,TResource>(x)).ToList(),
                    Links = LinkCollection.Self(_helper.ResolveLink(linkRouteName)).AddPaging(skip,this.Options.ItemsPerPage,totalResults,(s,t) => _helper.ResolveLink(linkRouteName,new {skip = s,take = t}))
                };
                return collection;
            }
        }
    }
}
