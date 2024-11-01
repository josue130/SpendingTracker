using SpendingTracker.Application.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task Register(RegisterRequestDto request);
    }
}
