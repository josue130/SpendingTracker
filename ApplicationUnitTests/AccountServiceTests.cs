using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services;
using SpendingTracker.Application.Services.IServices;
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
    public class AccountServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthService> _authServiceMock;
        private AccountService _accountService;
        public AccountServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authServiceMock = new Mock<IAuthService>();
            _mapperMock = new Mock<IMapper>();
            _accountService = new AccountService(_unitOfWorkMock.Object,_mapperMock.Object,_authServiceMock.Object);
        }
        private ClaimsPrincipal CreateUserClaimsPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));
        }

        [Fact]
        public async Task GetUserAccounts_WithValidUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            AccountsDto model = new AccountsDto { AccountName = "TestName", Amount = 100, Description = "TestDescription" };
            List<AccountsDto> data = new() {model};

            _authServiceMock.Setup(s => s.CheckUserId(user))
                .Returns(Result<Guid>.Success(userId));

            _unitOfWorkMock.Setup(u => u.userAccounts.GetUserAccounts(userId))
                .ReturnsAsync(data);

            //Act
            var result = await _accountService.GetAccountbyUserId(user);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public async Task GetUserAccounts_WithInValidUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            _authServiceMock.Setup(s => s.CheckUserId(user))
                .Returns(Result.Failure<Guid>(JWTError.JwtTokenInvalid));

            //Act
            var result = await _accountService.GetAccountbyUserId(user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(JWTError.JwtTokenInvalid);
        }
        [Fact]
        public async Task CreateAccount_WithValidInputs_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            AccountsDto model = new AccountsDto {AccountName = "TestName",Amount=100,Description="TestDescription" };

            _authServiceMock.Setup(s => s.CheckUserId(user))
                .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.accounts.Add(It.IsAny<Accounts>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.userAccounts.Add(It.IsAny<UserAccounts>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);


            //Act
            var result = await _accountService.CreatAccount(model,user);

            //Assert
            result.IsSuccess.Should().BeTrue();

        }
        [Fact]
        public async Task CreateAccount_WithInvalidAmount_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            AccountsDto model = new AccountsDto { AccountName = "TestName", Amount = -100, Description = "TestDescription" };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));

            //Act
            var result = await _accountService.CreatAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountsError.InvalidAmount);
        }
        [Fact]
        public async Task CreateAccount_WithInvalidName_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            AccountsDto model = new AccountsDto { AccountName = " ", Amount = 100, Description = "TestDescription" };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));

            //Act
            var result = await _accountService.CreatAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);
        }
        [Fact]
        public async Task CreateAccount_WithInvalidDescription_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            AccountsDto model = new AccountsDto { AccountName = "TestName", Amount = 100, Description = "  " };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));

            //Act
            var result = await _accountService.CreatAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);
        }
        [Fact]
        public async Task DeleteAccount_WithAuthorizedUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = "TestName", Amount = 100, Description = "  " };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);
            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()));

            //Act
            var result = await _accountService.RemoveAccount(accountId,user);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteAccount_WithUnAuthorizedUser_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = "TestName", Amount = 100, Description = "  " };
 

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()));
     

            //Act
            var result = await _accountService.RemoveAccount(accountId, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountsError.AccountNotFound);
        }
        [Fact]
        public async Task UpdateAccount_WithValidInputs_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = "TestName", Amount = 100, Description = "TestDescription" };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);
            _unitOfWorkMock.Setup(u => u.accounts.Add(It.IsAny<Accounts>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);

            //Act
            var result = await _accountService.UpdateAccount(model,user);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public async Task UpdateAccount_WithInvalidAmount_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = "TestName", Amount = -100, Description = "TestDescription" };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);


            //Act
            var result = await _accountService.UpdateAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AccountsError.InvalidAmount);
        }
        [Fact]
        public async Task UpdateAccount_WithValidInvalidName_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = " ", Amount = 100, Description = "TestDescription" };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);
  

            //Act
            var result = await _accountService.UpdateAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);
        }
        [Fact]
        public async Task UpdateAccount_WithValidInvalidDescription_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            AccountsDto model = new AccountsDto { Id = accountId, AccountName = "TestName ", Amount = 100, Description = "" };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };

            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);


            //Act
            var result = await _accountService.UpdateAccount(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);
        }

    }
}
