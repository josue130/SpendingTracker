using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Infrastructure.Data;
using System.Linq.Expressions;


namespace SpendingTracker.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            var entity = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);

        }
    }
}
