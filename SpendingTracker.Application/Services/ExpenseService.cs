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
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

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
                return Result.Failure(GlobalError.InvalidInputs);
            }
            Expense expense = Expense.Create(model.Description, model.Amount, model.Date, model.AccountId, model.CategoryId);
            await _unitOfWork.expense.Add(expense);
            await _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> DeleteExpense(Guid Id, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            bool result = await _unitOfWork.expense.CheckUserAccess(Id, userId);
            if (!result)
            {
                return Result.Failure(ExpenseError.ExpenseNotFound);
            }

            Expense expense = await _unitOfWork.expense.Get(i => i.Id == Id);
            _unitOfWork.expense.Remove(expense);
            await _unitOfWork.Save();
            return Result.Success();
        }

        public async Task<Result> GetExpense(Guid accountId, int month, int year)
        {
            IEnumerable<Expense> response = await _unitOfWork.expense.GetExpenseByAccountId(accountId, month, year);
            return Result.Success(_mapper.Map<IEnumerable<ExpenseDto>>(response));
        }

        public async Task<Result> UpdateExpense(ExpenseDto model, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            bool result = await _unitOfWork.expense.CheckUserAccess(model.Id, userId);
            if (!result)
            {
                return Result.Failure(ExpenseError.ExpenseNotFound);
            }

            if (model.CategoryId == Guid.Empty
                    || model.AccountId == Guid.Empty || model.Date == default)
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            if (model.Amount < 0)
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            _unitOfWork.expense.Update(_mapper.Map<Expense>(model));
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
