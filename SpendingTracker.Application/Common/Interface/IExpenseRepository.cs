using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Interface
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<bool> CheckUserAccess(Guid expenseId, Guid userId);
        Task<IEnumerable<Expense>> GetExpenseByAccountId(Guid accountId, int month, int year);
        void Update(Expense expense);
    }
}
