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
    public interface ICategoryExpenseService
    {
        Task<Result> GetCategories(ClaimsPrincipal user);
        Task<Result> CreateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user);
        Task<Result> UpdateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user);
        Task<Result> DeleteCategoryExpense(Guid id, ClaimsPrincipal user);
    }
}
