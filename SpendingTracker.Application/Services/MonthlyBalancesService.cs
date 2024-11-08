using Microsoft.IdentityModel.Logging;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services
{
    public class MonthlyBalancesService : IMonthlyBalancesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MonthlyBalancesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> AddMonthlyBalance(Guid accountId, double amount, DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            MonthlyBalances monthlyBalances = await _unitOfWork.monthlyBalances.
                Get(mb => mb.AccountId == accountId && mb.Month == month && mb.Year == year);

            if (monthlyBalances == null)
            {
                monthlyBalances = MonthlyBalances.Create(year, month, amount, accountId);
                await _unitOfWork.monthlyBalances.Add(monthlyBalances);
            }
            else 
            {
                monthlyBalances.Balance += amount;
                _unitOfWork.monthlyBalances.Update(monthlyBalances);
            }
            
            return Result.Success();
        }
    }
}
