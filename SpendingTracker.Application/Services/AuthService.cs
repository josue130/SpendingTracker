using Microsoft.AspNetCore.Identity;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services.IServices;
using SpendingTracker.Domain.Entities;
using SpendingTracker.Domain.ValueObjects;
using System.Security.Claims;


namespace SpendingTracker.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<UserDto> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = new PasswordHasher<UserDto>();
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public Result<Guid> CheckUserId(ClaimsPrincipal user)
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Result.Failure<Guid>(JWTError.JwtTokenInvalid);
            }
            return Result.Success(userId);
        }

        public async Task<Result> Login(LoginRequestDto loginRequest)
        {
            Users user = await _unitOfWork.auth.Get(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());


            if (user == null)
            {
                return Result.Failure(AuthErrors.UserNameNotExist);
            }


            UserDto applicationUser = new UserDto
            {
                FullName = user.FullName,
                UserName = user.UserName
            };


            var result = _passwordHasher.VerifyHashedPassword(applicationUser, user.Password, loginRequest.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return Result.Failure(AuthErrors.IncorrectPassword);
            }


            return Result.Success(new LoginResponseDto
            {
                User = applicationUser,
                Token = _jwtTokenGenerator.GenerateToken(user)
            });
       
        }

        public async Task<Result> Register(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.UserName)
                || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result.Failure(GlobalError.InvalidInputs);
            }

            if (Email.Create(request.Email) is null)
            {
                return Result.Failure(AuthErrors.InvalidEmailFormat);
            }

            var applicationUser = new UserDto()
            {
                FullName = request.FullName,
                UserName = request.UserName
            };

            var user = await _unitOfWork.auth.Get(u => u.UserName.ToLower() == request.UserName.ToLower());
            if (user != null)
            {
                return Result.Failure(AuthErrors.UserNameAlreadyExits);
            }

            var hashPassword = _passwordHasher.HashPassword(applicationUser, request.Password);
            Users newUser = Users.Create(request.FullName, request.UserName,request.Email, hashPassword);

            await _unitOfWork.auth.Add(newUser);
            await _unitOfWork.Save();


            return Result.Success("User registered succesfully");

        }
    }
}
