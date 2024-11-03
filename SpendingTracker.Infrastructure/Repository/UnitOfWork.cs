﻿
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Infrastructure.Data;
using SpendingTracker.Infrastructure.Repository;

namespace Workout.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IAuthRepository auth { get; init; }
        public IAccountRepository accounts { get; init; }
        public IUserAccountRepository userAccounts { get; init; }
        public IUserIncomeRepository userIncome { get; init; }
        public ICategoryIncomeRepository categoryIncome { get; init; }
        public IIncomeRepository income { get; init; }

    public UnitOfWork(AppDbContext db)
        {
            _db = db;
            auth = new AuthRepository(db);
            accounts = new AccountRepository(db);
            userAccounts = new UserAccountRepository(db);
            income = new IncomeRepository(db);
            categoryIncome = new CategoryIncomeRepository(db);
            userIncome = new UserIncomeRepository(db);

        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
