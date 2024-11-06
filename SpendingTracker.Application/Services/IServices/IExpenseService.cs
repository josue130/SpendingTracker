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
    public interface IExpenseService
    {
        Task<Result> GetExpense(Guid accountId, int month, int year);
        Task<Result> AddExpense(ExpenseDto model, ClaimsPrincipal user);
        Task<Result> UpdateExpense(ExpenseDto model, ClaimsPrincipal user);
        Task<Result> DeleteExpense(Guid Id, ClaimsPrincipal user);
    }
}
