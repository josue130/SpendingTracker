using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services.IServices
{
    public interface IIncomeService
    {
        Task<Result> GetIncome(Guid accountId, int month, int year);
        Task<Result> AddIncome(IncomeDto model, ClaimsPrincipal user);
        Task<Result> UpdateIncome(IncomeDto model, ClaimsPrincipal user);
        Task<Result> DeleteIncome(Guid Id, ClaimsPrincipal user);

    }
}
