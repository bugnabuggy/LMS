using System.Linq;
using LMS.Core;
using Microsoft.Data.Entity;

namespace LMS.Infrastructure
{
    public class Repository<TModel> : IRepository<TModel>
        where TModel : class
    {
        private DbContext _context;

        public IQueryable<TModel> Items => _dbSet;

        private readonly DbSet<TModel> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }

        public TModel Add(TModel item)
        {
            _dbSet.Add(item);

            return item;
        }

        public TModel Update(TModel item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _context.Attach(item);
            }
            _dbSet.Update(item);

            return item;
        }

        public void Delete(TModel item)
        {
            _dbSet.Remove(item);
        }
    }
}
