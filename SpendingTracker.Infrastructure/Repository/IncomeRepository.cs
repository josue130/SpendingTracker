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
        public IncomeRepository(AppDbContext db) : base(db)
        {
        }
    }
}
