using Microsoft.AspNetCore.Identity;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services;
using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUnitTests
{
    public class AuthServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private AuthService _authService;
        private readonly PasswordHasher<UserDto> _passwordHasher;
        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _authService = new AuthService(_unitOfWorkMock.Object,_jwtTokenGeneratorMock.Object);
            _passwordHasher = new PasswordHasher<UserDto>();
        }

        [Fact]
        public async Task RegisterNewUser_WithValidInputs_ShouldReturnSuccessResult() 
        {
            //Arrange
            RegisterRequestDto request = new() {UserName="Test",Email="test@gmail.com",FullName="Test",Password="Test76t" };

            _unitOfWorkMock.Setup(u => u.auth.Get(It.IsAny<Expression<Func<Users, bool>>>()))
                .ReturnsAsync((Users)null);
            _unitOfWorkMock.Setup(u => u.auth.Add(It.IsAny<Users>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);
            //Act
            var result = await _authService.Register(request);


            //Assert
            result.IsSuccess.Should().BeTrue();


        }

        [Fact]
        public async Task RegisterNewUser_WithInvalidValidEmail_ShouldReturnFailureResult()
        {
            //Arrange
            RegisterRequestDto request = new() { UserName = "Test", Email = "testgmail.com", FullName = "Test", Password = "Test76t" };

            
            //Act
            var result = await _authService.Register(request);


            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.InvalidEmailFormat);


        }
        [Fact]
        public async Task RegisterNewUser_WithInvalidValidFullName_ShouldReturnFailureResult()
        {
            //Arrange
            RegisterRequestDto request = new() { UserName = "Test", Email = "test@gmail.com", FullName = " ", Password = "Test76t" };


            //Act
            var result = await _authService.Register(request);


            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);


        }
        [Fact]
        public async Task RegisterNewUser_WithInvalidValidPassword_ShouldReturnFailureResult()
        {
            //Arrange
            RegisterRequestDto request = new() { UserName = "Test", Email = "test@gmail.com", FullName = "test", Password = " " };


            //Act
            var result = await _authService.Register(request);


            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);
        }

        [Fact]
        public async Task RegisterNewUser_WithInvalidValidUserName_ShouldReturnFailureResult()
        {
            //Arrange
            RegisterRequestDto request = new() { UserName = "Test", Email = "test@gmail.com", FullName = "test", Password = "test" };
            Users model = Users.Create(request.FullName,request.UserName,request.Email,request.Password);
            _unitOfWorkMock.Setup(u => u.auth.Get(It.IsAny<Expression<Func<Users, bool>>>()))
                .ReturnsAsync(model);

            //Act
            var result = await _authService.Register(request);


            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.UserNameAlreadyExits);
        }

        [Fact]
        public async Task LoginUser_WithValidCredentials_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            LoginRequestDto request = new() {UserName="UserTest",Password="T1s2t89-" };
            var applicationUser = new UserDto()
            {
                FullName = "FullName",
                UserName = request.UserName
            };
            var hashPassword = _passwordHasher.HashPassword(applicationUser, request.Password);
            Users model = new Users(userId, "FullName", "UserTest", "email@gmail.com", hashPassword);

            _unitOfWorkMock.Setup(u => u.auth.Get(It.IsAny<Expression<Func<Users, bool>>>()))
                .ReturnsAsync(model);

            //Act
            var result = await _authService.Login(request);

            //Assert
            result.IsSuccess.Should().BeTrue();

        }
        [Fact]
        public async Task LoginUser_WithInvalidPassword_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            LoginRequestDto request = new() { UserName = "UserTest", Password = "T1s2t89-" };
            var applicationUser = new UserDto()
            {
                FullName = "FullName",
                UserName = request.UserName
            };
            var hashPassword = _passwordHasher.HashPassword(applicationUser, "V1st89-");
            Users model = new Users(userId, "FullName", "UserTest", "email@gmail.com", hashPassword);

            _unitOfWorkMock.Setup(u => u.auth.Get(It.IsAny<Expression<Func<Users, bool>>>()))
                .ReturnsAsync(model);

            //Act
            var result = await _authService.Login(request);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.IncorrectPassword);
        }
        [Fact]
        public async Task LoginUser_WithInvalidUserName_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            LoginRequestDto request = new() { UserName = "UserTest", Password = "T1s2t89-" };
           
            _unitOfWorkMock.Setup(u => u.auth.Get(It.IsAny<Expression<Func<Users, bool>>>()))
                .ReturnsAsync((Users)null);

            //Act
            var result = await _authService.Login(request);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.UserNameNotExist);
        }
    }
}
