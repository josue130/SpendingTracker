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
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMonthlyBalancesService _monthlyBalancesService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper, IMonthlyBalancesService monthlyBalancesService, IAuthService authService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _monthlyBalancesService = monthlyBalancesService;
            _authService = authService;
        }
        public async Task<Result> AddExpense(ExpenseDto model, ClaimsPrincipal user)
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

            Expense expense = Expense.Create(model.Description, model.Amount, model.Date, model.AccountId, model.CategoryId);
            await _unitOfWork.expense.Add(expense);

            //Update account amount
            Accounts accounts = await _unitOfWork.accounts.Get(a => a.Id == model.AccountId);
            accounts.Amount -= model.Amount;
            _unitOfWork.accounts.Update(accounts);


            await _monthlyBalancesService.AddMonthlyBalance(model.AccountId, model.Amount * -1, model.Date);

            await _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> DeleteExpense(Guid Id, ClaimsPrincipal user)
        {
            var result = await ValidateExpenseAccess(user, Id);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            Expense expense = await _unitOfWork.expense.Get(i => i.Id == Id);
            _unitOfWork.expense.Remove(expense);

            //Update account amount
            Accounts accounts = await _unitOfWork.accounts.Get(a => a.Id == expense.AccountId);
            accounts.Amount += expense.Amount;
            _unitOfWork.accounts.Update(accounts);


            await _monthlyBalancesService.AddMonthlyBalance(expense.AccountId, expense.Amount, expense.Date);

            await _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> GetExpense(Guid accountId, int month, int year, ClaimsPrincipal user)
        {
            var accessResult = await ValidateAccountAccess(user, accountId);
            if (accessResult.IsFailure)
            {
                return Result.Failure(accessResult.Error);
            }
            IEnumerable<Expense> response = await _unitOfWork.expense.GetExpenseByAccountId(accountId, month, year);
            return Result.Success(_mapper.Map<IEnumerable<ExpenseDto>>(response));
        }

        public async Task<Result> UpdateExpense(ExpenseDto model, ClaimsPrincipal user)
        {
            var result = await ValidateExpenseAccess(user,model.Id);
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
            Expense expense = Expense.Update(model.Id,model.Description,model.Amount,model.Date,model.AccountId,model.CategoryId);
            _unitOfWork.expense.Update(expense);

            //Update account amount
            expense = await _unitOfWork.expense.Get(ex => ex.Id == model.Id);
            double diff = expense.Amount - model.Amount;
            Accounts accounts = await _unitOfWork.accounts.Get(a => a.Id == model.AccountId);
            accounts.Amount += diff;
            _unitOfWork.accounts.Update(accounts);


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

        private async Task<Result> ValidateExpenseAccess(ClaimsPrincipal user,Guid expenseId) 
        {
            Result<Guid> userId = _authService.CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure(userId.Error);
            }
            bool result = await _unitOfWork.expense.CheckUserAccess(expenseId, userId.Value);
            if (!result)
            {
                return Result.Failure(ExpenseError.ExpenseNotFound);
            }
            return Result.Success();
        }
    }
}
