using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRouteDescriptor.Data
{
    public interface IDataStore: IDisposable
    {
        IQueryable<TDocument> Query<TDocument>() where TDocument : class;
        void Commit();
    }

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

        public void Commit()
        {
            
        }
    }
}
