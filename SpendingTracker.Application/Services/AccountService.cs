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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public  async Task<Result> CreatAccount(AccountsDto model, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            if (string.IsNullOrWhiteSpace(model.Description) || string.IsNullOrWhiteSpace(model.AccountName))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }
            if (model.Amount <= 0)
            {
                return Result.Failure(AccountsError.InvalidAmount);
            }
            Accounts account = Accounts.Create(model.AccountName,model.Amount,model.Description);
            await _unitOfWork.accounts.Add(account);
            var userAccount = new UserAccounts() { Id = Guid.NewGuid(), AccountId = account.Id, UserId = userId };
            await _unitOfWork.userAccounts.Add(userAccount);
            await _unitOfWork.Save();

            return Result.Success("Account created successfully");
        }

        public async Task<Result>GetAccountbyUserId(ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            var response = await _unitOfWork.userAccounts.GetUserAccounts(userId);
            return Result.Success(response);
        }

        public async Task<Result> RemoveAccount(Guid accountId, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            Result result = await CheckUserAccess(accountId, userId);

            if (result.IsFailure)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }
            Accounts account = await _unitOfWork.accounts.Get(account => account.Id == accountId);
            _unitOfWork.accounts.Remove(account);
            await _unitOfWork.Save();
            return Result.Success("Account deleted successfully.");
        }

        public async Task<Result> UpdateAccount(AccountsDto model, ClaimsPrincipal user)
        {
            Guid userId = CheckUserId(user);
            Result result = await CheckUserAccess(model.Id, userId);

            if (result.IsFailure)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }

            if (string.IsNullOrWhiteSpace(model.Description) || string.IsNullOrWhiteSpace(model.AccountName))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }

            _unitOfWork.accounts.Update(_mapper.Map<Accounts>(model));
            await _unitOfWork.Save();
           
            return Result.Success("Account updated successfully.");

        }


        private async Task<Result> CheckUserAccess(Guid accountId, Guid userId)
        {
            UserAccounts userAccount = await _unitOfWork.userAccounts.Get(ua => ua.AccountId == accountId && ua.UserId == userId);

            if (userAccount == null)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }
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
