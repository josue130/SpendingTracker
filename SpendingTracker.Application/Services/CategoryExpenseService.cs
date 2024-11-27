using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using System.Reflection;
using System.Security.Claims;


namespace SpendingTracker.Application.Services
{
    public class CategoryExpenseService : ICategoryExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public CategoryExpenseService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;

        }
        public async Task<Result> CreateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            Result<Guid> result = CheckUserId(user);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            CategoryExpense categoryExpense = CategoryExpense.Create(model.CategoryName, model.Color, model.Icon, result.Value);
            await _unitOfWork.categoryExpense.Add(categoryExpense);
            await _unitOfWork.Save();
            return Result.Success("Category created successfully");
        }

        public async Task<Result> DeleteCategoryExpense(Guid id, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(id, user);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            CategoryExpense categoryExpense = await _unitOfWork.categoryExpense.Get(ui => ui.Id == id);
            _unitOfWork.categoryExpense.Remove(categoryExpense);
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
            IEnumerable<CategoryExpense> response = await _unitOfWork.categoryExpense.GetCategories(result.Value);
            return Result.Success(_mapper.Map<IEnumerable<CategoryExpenseDto>>(response));
        }

        public async Task<Result> UpdateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(model.Id,user);
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
            CategoryExpense categoryExpense = CategoryExpense.Update(model.Id,model.CategoryName,model.Color,model.Icon,model.UserId);
            _unitOfWork.categoryExpense.Update(categoryExpense);
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
            CategoryExpense response = await _unitOfWork.categoryExpense.Get(ua => ua.Id == id && ua.UserId == userId.Value);

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
