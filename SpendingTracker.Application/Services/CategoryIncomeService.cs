using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services
{
    public class CategoryIncomeService : ICategoryIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CategoryIncomeService(IUnitOfWork unitOfWork, IMapper mapper,IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<Result> CreateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user)
        {
            Result<Guid> result = CheckUserId(user);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
         
            CategoryIncome categoryIncome = CategoryIncome.Create(model.CategoryName, model.Color, model.Icon,result.Value );
            await _unitOfWork.categoryIncome.Add(categoryIncome);
            await _unitOfWork.Save();
            return Result.Success("Category created successfully");
        }

        public async Task<Result> DeleteCategoryIncome(Guid id, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(id, user);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            CategoryIncome categoryIncome = await _unitOfWork.categoryIncome.Get(ui => ui.Id == id);
            _unitOfWork.categoryIncome.Remove(categoryIncome);
            await _unitOfWork.Save();
            return Result.Success("category deleted successfully");
        }

        public async Task<Result> GetCategories(ClaimsPrincipal user)
        {
            Result<Guid> result = CheckUserId(user);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            IEnumerable<CategoryIncome> response = await _unitOfWork.categoryIncome.GetCategories(result.Value);
            return Result.Success(_mapper.Map<IEnumerable<CategoryIncomeDto>>(response));
        }

        public async Task<Result> UpdateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(model.Id, user);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            model.UserId = result.Value;
            CategoryIncome categoryIncome = CategoryIncome.Update(model.Id,model.CategoryName,model.Color,model.Icon,model.UserId);
            _unitOfWork.categoryIncome.Update(_mapper.Map<CategoryIncome>(model));
            await _unitOfWork.Save();
            return Result.Success("Category updated successfully");
        }


        private async Task<Result<Guid>> CheckUserAccess(Guid id, ClaimsPrincipal user)
        {
            Result<Guid> userId = CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure<Guid>(userId.Error);
            }
            CategoryIncome response = await _unitOfWork.categoryIncome.Get(ua => ua.Id == id && ua.UserId == userId.Value);

            if (response == null)
            {
                return Result.Failure<Guid>(CategoryExpenseError.CategoryNotFound);
            }
            return Result.Success(userId.Value);
        }

        private Result<Guid> CheckUserId(ClaimsPrincipal user)
        {
            Result<Guid> userId = _authService.CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure<Guid>(userId.Error);
            }
            return Result.Success(userId.Value);
        }
    }
}
