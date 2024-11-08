using SpendingTracker.Application.Common.Result;
using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services.IServices
{
    public interface IMonthlyBalancesService
    {
        Task<Result> AddMonthlyBalance(Guid accountId, double amount, DateTime date);
    }
}
