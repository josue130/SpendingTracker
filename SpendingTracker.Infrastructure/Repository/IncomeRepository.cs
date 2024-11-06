using Microsoft.EntityFrameworkCore;
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
    public class IncomeRepository : Repository<Income>, IIncomeRepository
    {
        private readonly AppDbContext _db;
        public IncomeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> CheckUserAccess(Guid incomeId, Guid userId)
        {
            var income = await _db.income
                .AsNoTracking()
                .Where(i => i.Id == incomeId)
                .Join(_db.userAccounts,
                    i => i.AccountId,
                    ua => ua.AccountId,
                    (i, ua) => new { Income = i, UserAccount = ua })
                .FirstOrDefaultAsync(x => x.UserAccount.UserId == userId);
            if (income == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Income>> GetIncomesByAccountId(Guid accountId, int month, int year)
        {
            return await _db.income.Where(i => i.AccountId == accountId &&
                i.Date.Year == year &&
                i.Date.Month == month)
                .ToListAsync();
        }

        public void Update(Income income)
        {
            _db.income.Update(income);
        }
    }
}
