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
    public interface ICategoryIncomeService
    {
        Task<Result> GetCategories(ClaimsPrincipal user);
        Task<Result> CreateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user);
        Task<Result> UpdateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user);
        Task<Result> DeleteCategoryIncome(Guid id, ClaimsPrincipal user);
    }
}
