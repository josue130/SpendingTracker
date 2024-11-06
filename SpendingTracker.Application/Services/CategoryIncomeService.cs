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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services
{
    public class CategoryIncomeService : ICategoryIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryIncomeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            Guid userId = CheckUserId(user);
            CategoryIncome categoryIncome = CategoryIncome.Create(model.CategoryName, model.Color, model.Icon,userId );
            await _unitOfWork.categoryIncome.Add(categoryIncome);
            await _unitOfWork.Save();
            return Result.Success("Category created successfully");
        }

        public async Task<Result> DeleteCategoryIncome(Guid id, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            Result result = await CheckUserAccess(id, userId);

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
            Guid userId = CheckUserId(user);
            IEnumerable<CategoryIncome> response = await _unitOfWork.categoryIncome.GetCategories(userId);
            return Result.Success(_mapper.Map<IEnumerable<CategoryIncomeDto>>(response));
        }

        public async Task<Result> UpdateCategoryIncome(CategoryIncomeDto model, ClaimsPrincipal user)
        {
            if (string.IsNullOrWhiteSpace(model.Color) || string.IsNullOrWhiteSpace(model.CategoryName)
                || string.IsNullOrWhiteSpace(model.Icon))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            _unitOfWork.categoryIncome.Update(_mapper.Map<CategoryIncome>(model));
            await _unitOfWork.Save();
            return Result.Success("Category updated successfully");
        }


        private async Task<Result> CheckUserAccess(Guid id, Guid userId)
        {
            CategoryIncome response = await _unitOfWork.categoryIncome.Get(ua => ua.Id == id && ua.UserId == userId);

            if (response == null)
            {
                return Result.Failure(CategoryIncomeError.CategorytNotFound);
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
