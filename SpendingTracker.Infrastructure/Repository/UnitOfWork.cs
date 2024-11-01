
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Infrastructure.Data;
using SpendingTracker.Infrastructure.Repository;

namespace Workout.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IAuthRepository auth { get; init; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            auth = new AuthRepository(db);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
