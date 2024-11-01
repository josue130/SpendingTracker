﻿using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services.IServices
{
    public interface IAuthService
    {
        Task<Result> Login(LoginRequestDto loginRequest);
        Task<Result> Register(RegisterRequestDto request);
    }
}
