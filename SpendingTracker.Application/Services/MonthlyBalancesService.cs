using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.IdentityModel.Logging;
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
    public class MonthlyBalancesService : IMonthlyBalancesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public MonthlyBalancesService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;

        }
        public async Task<Result> AddMonthlyBalance(Guid accountId ,double amount, DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            MonthlyBalances monthlyBalances = await _unitOfWork.monthlyBalances.
                Get(mb => mb.AccountId == accountId && mb.Month == month && mb.Year == year);

            if (monthlyBalances == null)
            {
                monthlyBalances = MonthlyBalances.Create(year, month, amount, accountId);
                await _unitOfWork.monthlyBalances.Add(monthlyBalances);
            }
            else 
            {
                monthlyBalances.Balance += amount;
                _unitOfWork.monthlyBalances.Update(monthlyBalances);
            }
            
            return Result.Success();
        }

        public async Task<Result> GetMonthlyBalance(Guid accountId,int year, int month, ClaimsPrincipal user)
        {
            Result<Guid> getUserId = CheckUserId(user);
            if (getUserId.IsFailure)
            {
                return Result.Failure<Guid>(getUserId.Error);
            }
            var access = await _unitOfWork.userAccounts.
                Get(ua => ua.AccountId == accountId 
                && ua.UserId == getUserId.Value);

            if (access == null)
            {
                return Result.Failure(AccountsError.AccountNotFound);
            }

            MonthlyBalances result = await _unitOfWork.monthlyBalances.
                Get(mb => mb.AccountId == accountId
                && mb.Year == year && mb.Month == month);

            if (result == null)
            {
                return Result.Success(0);
            }
            return Result.Success(result.Balance);
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
