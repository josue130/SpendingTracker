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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IMonthlyBalancesService _monthlyBalancesService;
        private readonly IMapper _mapper;
        public IncomeService(IUnitOfWork unitOfWork, IMapper mapper, IMonthlyBalancesService monthlyBalancesService, IAuthService authService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _monthlyBalancesService = monthlyBalancesService;
            _authService = authService;

        }
        public async Task<Result> AddIncome(IncomeDto model, ClaimsPrincipal user)
        {

            if (model.CategoryId == Guid.Empty
                    || model.AccountId == Guid.Empty || model.Date == default)
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            if (model.Amount < 0)
            {
                return Result.Failure(GlobalError.InvalidAmount);
            }

            var accessResult = await ValidateAccountAccess(user, model.AccountId);
            if (accessResult.IsFailure)
            {
                return Result.Failure(accessResult.Error);
            }

            Income income = Income.Create(model.Description, model.Amount, model.Date, model.AccountId, model.CategoryId);
            await _unitOfWork.income.Add(income);

            //Update account amount
            Accounts accounts = await _unitOfWork.accounts.Get(a => a.Id == model.AccountId);
            accounts.Amount += model.Amount;
            _unitOfWork.accounts.Update(accounts);

            await _monthlyBalancesService.AddMonthlyBalance(model.AccountId, model.Amount, model.Date);

            await _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> DeleteIncome(Guid Id, ClaimsPrincipal user)
        {
            var result = await ValidateIncomeAccess(user, Id);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Income income = await _unitOfWork.income.Get(i => i.Id == Id);
            _unitOfWork.income.Remove(income);


            //Update account amount 
            Accounts accounts = await _unitOfWork.accounts.Get(a => a.Id == income.AccountId);
            accounts.Amount -= income.Amount;
            _unitOfWork.accounts.Update(accounts);

            await _monthlyBalancesService.AddMonthlyBalance(income.AccountId, income.Amount * -1, income.Date);

            await _unitOfWork.Save();
            return Result.Success();

        }

        public async Task<Result> GetIncome(Guid accountId, int month, int year, ClaimsPrincipal user)
        {
            var accessResult = await ValidateAccountAccess(user, accountId);
            if (accessResult.IsFailure)
            {
                return Result.Failure(accessResult.Error);
            }
            IEnumerable<Income> response = await _unitOfWork.income.GetIncomesByAccountId(accountId,month,year);
            return Result.Success(_mapper.Map<IEnumerable<IncomeDto>>(response));
        }

        public async Task<Result> UpdateIncome(IncomeDto model, ClaimsPrincipal user)
        {
            var result = await ValidateIncomeAccess(user, model.Id);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }


            if (model.CategoryId == Guid.Empty
                    || model.AccountId == Guid.Empty || model.Date == default)
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            if (model.Amount < 0)
            {
                return Result.Failure(GlobalError.InvalidAmount);
            }
            Income income = Income.Update(model.Id,model.Description,model.Amount,model.Date,model.AccountId,model.CategoryId);
            _unitOfWork.income.Update(income);


            //Update account amount
            income = await _unitOfWork.income.Get(a => a.Id == model.Id);
            double diff = model.Amount - income.Amount;
            Accounts account = await _unitOfWork.accounts.Get(a => a.Id == model.AccountId);
            account.Amount += diff;
            _unitOfWork.accounts.Update(account);


            await _monthlyBalancesService.AddMonthlyBalance(model.AccountId, diff, model.Date);

            await _unitOfWork.Save();
            return Result.Success();

        }
        private async Task<Result> ValidateAccountAccess(ClaimsPrincipal user, Guid accountId)
        {
            Result<Guid> userId = _authService.CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure(userId.Error);
            }
            UserAccounts userAccounts = await _unitOfWork.userAccounts.Get(ua => ua.UserId == userId.Value
                && ua.AccountId == accountId);

            if (userAccounts == null)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }
            return Result.Success();
        }

        private async Task<Result> ValidateIncomeAccess(ClaimsPrincipal user, Guid expenseId)
        {
            Result<Guid> userId = _authService.CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure(userId.Error);
            }
            bool result = await _unitOfWork.income.CheckUserAccess(expenseId, userId.Value);
            if (!result)
            {
                return Result.Failure(IncomeError.IncomeNotFound);
            }
            return Result.Success();
        }
    }
}
