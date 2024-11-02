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
    public class AccountRepository : Repository<Accounts>, IAccountRepository
    {
        private AppDbContext _db;
        public AccountRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Accounts account)
        {
            _db.accounts.Update(account);
        }
    }
}
