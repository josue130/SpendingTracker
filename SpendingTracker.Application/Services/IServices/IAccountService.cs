using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services.IServices
{
    public interface IAccountService
    {
        Task<Result> CreatAccount(AccountsDto model, ClaimsPrincipal user);
        Task<Result> UpdateAccount(AccountsDto model, ClaimsPrincipal user);
        Task<Result> RemoveAccount(Guid accountId, ClaimsPrincipal user);
        Task<Result> GetAccountbyUserId(ClaimsPrincipal user);

    }
}
