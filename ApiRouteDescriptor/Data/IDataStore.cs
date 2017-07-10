using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRouteDescriptor.Data
{
    public interface IDataStore: IDisposable
    {
        IQueryable<TDocument> Query<TDocument>() where TDocument : class;
        TDocument Load<TDocument, TId>(TId id) where TDocument : class, IHaveId<TId>;
        void Commit();
    }
}
