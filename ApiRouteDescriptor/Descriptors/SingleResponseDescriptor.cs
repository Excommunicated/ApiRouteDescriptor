using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiRouteDescriptor.Data;
using ApiRouteDescriptor.Resources;
using ApiRouteDescriptor.Responders;
using Microsoft.AspNetCore.Routing;

namespace ApiRouteDescriptor.Descriptors
{
    public class SingleResponseDescriptor<TModel,TResource, TId>: ModelResponseDescriptor where TModel: class, IHaveId<TId> where TResource: Resource
    {
        protected SingleResponseDescriptor(Type responderImplementationType) : base(typeof(TModel), responderImplementationType)
        {
            this.ResourceTypes.Add(typeof(TResource));
        }

        public SingleResponseDescriptor() : this(typeof(Responder))
        {
            
        }

        public class Responder : Responder<SingleResponseDescriptor<TModel, TResource,TId>>
        {
            private readonly IDataStore _dataStore;
            private readonly IResourceMapper _mapper;

            public Responder(IDataStore dataStore, IResourceMapper mapper)
            {
                _dataStore = dataStore;
                _mapper = mapper;
            }
            protected async override Task<Resource> Execute()
            {
                var id = this.Context.GetRouteValue("id");
                var result = _dataStore.Load<TModel,TId>((TId)id);
                return _mapper.MapTo<TModel, TResource>(result);
            }
        }
    }
}
