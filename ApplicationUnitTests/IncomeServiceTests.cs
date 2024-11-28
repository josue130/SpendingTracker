using AutoMapper;
using Moq;
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
using Xunit.Sdk;

namespace ApplicationUnitTests
{
    public class IncomeServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthService> _authServiceMock;
        private Mock<IMonthlyBalancesService> _balancesServiceMock;
        private IncomeService _incomeService;

        public IncomeServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authServiceMock = new Mock<IAuthService>();
            _mapperMock = new Mock<IMapper>();
            _balancesServiceMock = new Mock<IMonthlyBalancesService>();
            _incomeService = new IncomeService(_unitOfWorkMock.Object, _mapperMock.Object, _balancesServiceMock.Object, _authServiceMock.Object);
        }
        private ClaimsPrincipal CreateUserClaimsPrincipal(Guid userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));
        }
        [Fact]
        public async Task AddIncome_WithValidInputs_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            IncomeDto model = new() { Description = "Test", Amount = 100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };
            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);

            _unitOfWorkMock.Setup(u => u.income.Add(It.IsAny<Income>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Add(It.IsAny<MonthlyBalances>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);


            //Act 
            var result = await _incomeService.AddIncome(model, user);

            //Assert
            result.IsSuccess.Should().BeTrue();


        }

        [Fact]
        public async Task AddIncome_WithInValidAmount_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            IncomeDto model = new() { Description = "Test", Amount = -100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };

            //Act
            var result = await _incomeService.AddIncome(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidAmount);

        }
        [Fact]
        public async Task AddIncome_WithInValidAccountId_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            IncomeDto model = new() { Description = "Test", Amount = 100, Date = DateTime.Now, CategoryId = Guid.NewGuid() };

            //Act
            var result = await _incomeService.AddIncome(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidInputs);

        }
        [Fact]
        public async Task DeleteIncome_WithAuthorizedUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            Income model = new(Guid.NewGuid(), "Test", 100, DateTime.Now, accountId, Guid.NewGuid());

            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.income.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.income.Get(It.IsAny<Expression<Func<Income, bool>>>()))
                .ReturnsAsync(model);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);



            //Act
            var result = await _incomeService.DeleteIncome(model.Id, user);

            //Assert
            result.IsSuccess.Should().BeTrue();


        }
        [Fact]
        public async Task DeleteIncome_WithUnauthorizeduser_SholudReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            Income model = new(Guid.NewGuid(), "Test", 100, DateTime.Now, accountId, Guid.NewGuid());

            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.income.CheckUserAccess(model.Id, userId)).ReturnsAsync(false);

            //Act
            var result = await _incomeService.DeleteIncome(model.Id, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(IncomeError.IncomeNotFound);

        }
        [Fact]
        public async Task GetUserIncome_WithAuthorizedUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            Income model = new(Guid.NewGuid() , "Test", 100, DateTime.Now,accountId, Guid.NewGuid() );
            List<Income> data = new() { model };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));

            _unitOfWorkMock.Setup(u => u.income.GetIncomesByAccountId(accountId,11,2024))
                .ReturnsAsync(data);
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);

            //Act
            var result = await _incomeService.GetIncome(accountId,11,2024,user);

            //Assert
            result.IsSuccess.Should().BeTrue();

        }
        [Fact]
        public async Task UpdateIncome_WithValidInputs_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            IncomeDto modelDto = new() {Id=Guid.NewGuid(), Description = "Test", Amount = 100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };
            Income model = new(modelDto.Id, "Test", 100, DateTime.Now, accountId, Guid.NewGuid());
            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.income.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);
            _unitOfWorkMock.Setup(u => u.income.Get(It.IsAny<Expression<Func<Income, bool>>>()))
                .ReturnsAsync(model);
         


            //Act 
            var result = await _incomeService.UpdateIncome(modelDto, user);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
        [Fact]
        public async Task UpdateIncome_WithInvalidAmount_ShouldReturnFailureResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            IncomeDto model = new() { Id = Guid.NewGuid(), Description = "Test", Amount = -100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };
            
            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.income.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);

            //Act 
            var result = await _incomeService.UpdateIncome(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidAmount);
        }
    }
}
