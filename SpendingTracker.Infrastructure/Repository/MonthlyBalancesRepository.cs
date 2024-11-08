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
    public class MonthlyBalancesRepository : Repository<MonthlyBalances>, IMonthlyBalancesRepository
    {
        private AppDbContext _db;
        public MonthlyBalancesRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MonthlyBalances monthlyBalances)
        {
            _db.monthlyBalances.Update(monthlyBalances);
        }
    }
}
