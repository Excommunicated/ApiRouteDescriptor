using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ApiRouteDescriptor.Data
{
    public class InMemoryDataStore : IDataStore
    {
        ConcurrentDictionary<Type,IQueryable<object>> store = new ConcurrentDictionary<Type, IQueryable<object>>();

        public void Dispose()
        {
            
        }

        public IQueryable<TDocument> Query<TDocument>() where TDocument : class
        {
            if (!store.ContainsKey(typeof(TDocument)))
            {
                return Enumerable.Empty<TDocument>().AsQueryable();
            }
            return (IQueryable<TDocument>)store.GetOrAdd(typeof(TDocument), Enumerable.Empty<object>().AsQueryable());
        }

        public TDocument Load<TDocument, TId>(TId id) where TDocument : class, IHaveId<TId>
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            
        }
    }
}