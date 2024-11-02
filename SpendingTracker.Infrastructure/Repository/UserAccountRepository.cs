using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Infrastructure.Repository
{
    public class UserAccountRepository : Repository<UserAccounts>, IUserAccountRepository
    {
        private readonly AppDbContext _db;
        public UserAccountRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AccountsDto>> GetUserAccounts(Guid userId)
        {
            return await _db.userAccounts
                .AsNoTrackingWithIdentityResolution()
                .Where(ua=>ua.UserId == userId)
                .Select(ua=> new AccountsDto {
                    Id = ua.AccountId,
                    AccountName = ua.Accounts.AccountName,
                    Description = ua.Accounts.Description,
                    Amount = ua.Accounts.Amount
                }).ToListAsync();
        }
    }
}
