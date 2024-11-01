using Microsoft.AspNetCore.Identity;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Domain.ValueObjects;


namespace SpendingTracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<UserDto> _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = new PasswordHasher<UserDto>();
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            Users user = await _unitOfWork.auth.Get(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());


            if (user == null)
            {
               // error
            }


            UserDto applicationUser = new UserDto
            {
                FullName = user.FullName,
                UserName = user.UserName
            };


            var result = _passwordHasher.VerifyHashedPassword(applicationUser, user.Password, loginRequest.Password);

            if (result != PasswordVerificationResult.Success)
            {
                // error
            }

            

            return new LoginResponseDto
            {
                User = applicationUser,
                Token = ""
            };
        }

        public async Task Register(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.UserName)
                || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                // error
            }

            if (Email.Create(request.Email) is null)
            {
                // error
            }

            var applicationUser = new UserDto()
            {
                FullName = request.FullName,
                UserName = request.UserName
            };

            var user = await _unitOfWork.auth.Get(u => u.UserName.ToLower() == request.UserName.ToLower());
            if (user != null)
            {
               // error
            }

            var hashPassword = _passwordHasher.HashPassword(applicationUser, request.Password);
            Users newUser = Users.Create(request.FullName, request.UserName,request.Email, hashPassword);

            await _unitOfWork.auth.Add(newUser);
            await _unitOfWork.Save();

        }
    }
}
