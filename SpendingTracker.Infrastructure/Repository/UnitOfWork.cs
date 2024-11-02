
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Infrastructure.Data;
using SpendingTracker.Infrastructure.Repository;

namespace Workout.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IAuthRepository auth { get; init; }
        public IAccountRepository accounts { get; init; }
        public IUserAccountRepository userAccounts { get; init; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            auth = new AuthRepository(db);
            accounts = new AccountRepository(db);
            userAccounts = new UserAccountRepository(db);
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
