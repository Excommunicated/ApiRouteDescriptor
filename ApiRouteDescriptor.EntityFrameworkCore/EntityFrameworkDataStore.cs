using System;
using System.Linq;
using ApiRouteDescriptor.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiRouteDescriptor.EntityFrameworkCore
{
    public class EntityFrameworkDataStore<TContext> : IDataStore where TContext: DbContext
    {
        private readonly TContext _context;

        public EntityFrameworkDataStore(TContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public IQueryable<TDocument> Query<TDocument>() where TDocument : class
        {
            return _context.Set<TDocument>();
        }

        public TDocument Load<TDocument, TId>(TId id) where TDocument : class, IHaveId<TId>
        {
            return _context.Set<TDocument>().Find(id);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
