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
    public class IncomeService : IIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IncomeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        public async Task<Result> AddIncome(IncomeDto model, ClaimsPrincipal user)
        {

            if (model.CategoryId == Guid.Empty
                    || model.AccountId == Guid.Empty || model.Date == default)
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            if (model.Amount < 0)
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            Income income = Income.Create(model.Description, model.Amount, model.Date, model.AccountId, model.CategoryId);
            await _unitOfWork.income.Add(income);
            await _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> DeleteIncome(Guid Id, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            bool result = await _unitOfWork.income.CheckUserAccess(Id, userId);
            if (!result)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }

            Income income = await _unitOfWork.income.Get(i => i.Id == Id);
            _unitOfWork.income.Remove(income);
            await _unitOfWork.Save();
            return Result.Success();

        }

        public async Task<Result> GetIncome(Guid accountId, int month, int year)
        {
            IEnumerable<Income> response = await _unitOfWork.income.GetIncomesByAccountId(accountId,month,year);
            return Result.Success(_mapper.Map<IncomeDto>(response));
        }

        public async Task<Result> UpdateIncome(IncomeDto model, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            bool result = await _unitOfWork.income.CheckUserAccess(model.Id, userId);
            if (!result)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }

            if (model.CategoryId == Guid.Empty
                    || model.AccountId == Guid.Empty || model.Date == default)
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            if (model.Amount < 0)
            {
                return Result.Failure(CategoryIncomeError.InvalidInputs);
            }
            _unitOfWork.income.Update(_mapper.Map<Income>(model));
            await _unitOfWork.Save();
            return Result.Success();

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
