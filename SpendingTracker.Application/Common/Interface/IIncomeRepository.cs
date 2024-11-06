using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Interface
{
    public interface IIncomeRepository : IRepository<Income>
    {
        Task<bool> CheckUserAccess(Guid incomeId, Guid userId);

        Task<IEnumerable<Income>> GetIncomesByAccountId(Guid accountId, int month, int year);
        void Update(Income income);
    }
}
