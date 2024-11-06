using Microsoft.EntityFrameworkCore;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Infrastructure.Repository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private readonly AppDbContext _db;
        public ExpenseRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> CheckUserAccess(Guid expenseId, Guid userId)
        {
            var expense = await _db.expense
                .AsNoTracking()
                .Where(i => i.Id == expenseId)
                .Join(_db.userAccounts,
                    i => i.AccountId,
                    ua => ua.AccountId,
                    (i, ua) => new { Income = i, UserAccount = ua })
                .FirstOrDefaultAsync(x => x.UserAccount.UserId == userId);
            if (expense == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Expense>> GetExpenseByAccountId(Guid accountId, int month, int year)
        {
            return await _db.expense.Where(i => i.AccountId == accountId &&
                i.Date.Year == year &&
                i.Date.Month == month)
                .ToListAsync();
        }

        public void Update(Expense expense)
        {
            _db.expense.Update(expense);
        }
    }
}
