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
    public class CategoryExpenseRepository : Repository<CategoryExpense>, ICategoryExpenseRepository
    {
        private readonly AppDbContext _db;
        public CategoryExpenseRepository(AppDbContext db) : base(db)
        {
        }

        public void Update(CategoryExpense categoryExpense)
        {
            _db.categoryExpense.Update(categoryExpense);
        }
    }
}
