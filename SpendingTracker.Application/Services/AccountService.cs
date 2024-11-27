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
        private readonly IAuthService _authService;
        public AccountService(IUnitOfWork unitOfWork,IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public  async Task<Result> CreatAccount(AccountsDto model, ClaimsPrincipal user)
        {
            Result<Guid> result = CheckUserId(user);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
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
            var userAccount = new UserAccounts() { Id = Guid.NewGuid(), AccountId = account.Id, UserId = result.Value };
            await _unitOfWork.userAccounts.Add(userAccount);
            await _unitOfWork.Save();

            return Result.Success("Account created successfully");
        }

        public async Task<Result>GetAccountbyUserId(ClaimsPrincipal user)
        {
            Result<Guid> result = CheckUserId(user);
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
            var response = await _unitOfWork.userAccounts.GetUserAccounts(result.Value);
            return Result.Success(response);
        }

        public async Task<Result> RemoveAccount(Guid accountId, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(accountId, user);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
            Accounts account = await _unitOfWork.accounts.Get(account => account.Id == accountId);
            _unitOfWork.accounts.Remove(account);
            await _unitOfWork.Save();
            return Result.Success("Account deleted successfully.");
        }

        public async Task<Result> UpdateAccount(AccountsDto model, ClaimsPrincipal user)
        {
            Result<Guid> result = await CheckUserAccess(model.Id, user);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            if (string.IsNullOrWhiteSpace(model.Description) || string.IsNullOrWhiteSpace(model.AccountName))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }

            Accounts account = Accounts.Update(model.Id,model.AccountName,model.Amount,model.Description);
            _unitOfWork.accounts.Update(account);
            await _unitOfWork.Save();
           
            return Result.Success("Account updated successfully.");

        }


        private async Task<Result<Guid>> CheckUserAccess(Guid accountId,ClaimsPrincipal user)
        {
            Result<Guid> userId = CheckUserId(user);
            if (userId.IsFailure)
            {
                return Result.Failure<Guid>(userId.Error);
            }
            UserAccounts userAccount = await _unitOfWork.userAccounts.Get(ua => ua.AccountId == accountId && ua.UserId == userId.Value);

            if (userAccount == null)
            {
                return Result.Failure<Guid>(AccountsError.AccountNotFound);
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
