using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using System.Security.Claims;


namespace SpendingTracker.Application.Services
{
    public class CategoryExpenseService : ICategoryExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            Guid userId = CheckUserId(user);
            CategoryExpense categoryExpense = CategoryExpense.Create(model.CategoryName, model.Color, model.Icon, userId);
            await _unitOfWork.categoryExpense.Add(categoryExpense);
            await _unitOfWork.Save();
            return Result.Success("Category created successfully");
        }

        public async Task<Result> DeleteCategoryExpense(Guid id, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            Result result = await CheckUserAccess(id, userId);

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
            Guid userId = CheckUserId(user);
            IEnumerable<CategoryExpense> response = await _unitOfWork.categoryExpense.GetCategories(userId);
            return Result.Success(_mapper.Map<IEnumerable<CategoryExpenseDto>>(response));
        }

        public async Task<Result> UpdateCategoryExpense(CategoryExpenseDto model, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            _unitOfWork.categoryExpense.Update(_mapper.Map<CategoryExpense>(model));
            await _unitOfWork.Save();
            return Result.Success("Category updated successfully");
        }

        private async Task<Result> CheckUserAccess(Guid id, Guid userId)
        {
            CategoryExpense response = await _unitOfWork.categoryExpense.Get(ua => ua.Id == id && ua.UserId == userId);

            if (response == null)
            {
                return Result.Failure(CategoryExpenseError.CategoryNotFound);
            }
            return Result.Success(response);
        }

        private Guid CheckUserId(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                //Error or exeception
                throw new UnauthorizedAccessException();
            }
            return Guid.Parse(userId);
        }
    }
}
