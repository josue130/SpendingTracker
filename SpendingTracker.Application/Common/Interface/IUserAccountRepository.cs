﻿using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Interface
{
    public interface IUserAccountRepository : IRepository<UserAccounts>
    {
        Task<IEnumerable<AccountsDto>> GetUserAccounts(Guid userId);
    }
}
