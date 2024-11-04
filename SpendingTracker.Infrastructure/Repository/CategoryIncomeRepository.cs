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
    public class CategoryIncomeRepository : Repository<CategoryIncome>, ICategoryIncomeRepository
    {
        private readonly AppDbContext _db;
        public CategoryIncomeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryIncome>> GetCategories(Guid userId)
        {
            return await _db.categoryIncome.Where(ci => ci.UserId == userId).ToListAsync();
        }

        public void Update(CategoryIncome categoryIncome)
        {
            _db.categoryIncome.Update(categoryIncome);
        }
    }
}
