using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Interface
{
    public interface ICategoryExpenseRepository : IRepository<CategoryExpense>
    {
        Task<IEnumerable<CategoryExpense>> GetCategories(Guid userId)
        void Update(CategoryExpense categoryExpense);
    }
}
