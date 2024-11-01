using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Infrastructure.Data;


namespace SpendingTracker.Infrastructure.Repository
{
    public class AuthRepository : Repository<Users>, IAuthRepository
    {
        public AuthRepository(AppDbContext db) : base(db)
        {
        }
    }
}
