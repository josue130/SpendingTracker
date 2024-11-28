using AutoMapper;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Common.Interface;
using SpendingTracker.Application.Common.Result;
using SpendingTracker.Application.Errors;
using SpendingTracker.Application.Services.IServices;
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
    public class ExpenseServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthService> _authServiceMock;
        private Mock<IMonthlyBalancesService> _balancesServiceMock;
        private ExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authServiceMock = new Mock<IAuthService>();
            _mapperMock = new Mock<IMapper>();
            _balancesServiceMock = new Mock<IMonthlyBalancesService>();
            _expenseService = new ExpenseService(_unitOfWorkMock.Object, _mapperMock.Object, _balancesServiceMock.Object, _authServiceMock.Object);
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
            ExpenseDto model = new() { Description = "Test", Amount = 100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };
            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);

            _unitOfWorkMock.Setup(u => u.expense.Add(It.IsAny<Expense>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Add(It.IsAny<MonthlyBalances>()))
                           .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.Save()).Returns(Task.CompletedTask);


            //Act 
            var result = await _expenseService.AddExpense(model, user);

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
            ExpenseDto model = new() { Description = "Test", Amount = -100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };

            //Act
            var result = await _expenseService.AddExpense(model, user);

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
            ExpenseDto model = new() { Description = "Test", Amount = 100, Date = DateTime.Now, CategoryId = Guid.NewGuid() };

            //Act
            var result = await _expenseService.AddExpense(model, user);

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
            Expense model = new(Guid.NewGuid(), "Test", 100, DateTime.Now, accountId, Guid.NewGuid());

            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.expense.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.expense.Get(It.IsAny<Expression<Func<Expense, bool>>>()))
                .ReturnsAsync(model);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);



            //Act
            var result = await _expenseService.DeleteExpense(model.Id, user);

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
            _unitOfWorkMock.Setup(u => u.expense.CheckUserAccess(model.Id, userId)).ReturnsAsync(false);

            //Act
            var result = await _expenseService.DeleteExpense(model.Id, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ExpenseError.ExpenseNotFound);

        }
        [Fact]
        public async Task GetUserIncome_WithAuthorizedUser_ShouldReturnSuccessResult()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var user = CreateUserClaimsPrincipal(userId);
            Guid accountId = Guid.NewGuid();
            Expense model = new(Guid.NewGuid(), "Test", 100, DateTime.Now, accountId, Guid.NewGuid());
            List<Expense> data = new() { model };
            UserAccounts accessModel = new UserAccounts { Id = Guid.NewGuid(), AccountId = accountId, UserId = userId };
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));

            _unitOfWorkMock.Setup(u => u.expense.GetExpenseByAccountId(accountId, 11, 2024))
                .ReturnsAsync(data);
            _unitOfWorkMock.Setup(u => u.userAccounts.Get(It.IsAny<Expression<Func<UserAccounts, bool>>>()))
                .ReturnsAsync(accessModel);

            //Act
            var result = await _expenseService.GetExpense(accountId, 11, 2024, user);

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
            ExpenseDto modelDto = new() { Id = Guid.NewGuid(), Description = "Test", Amount = 100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };
            Expense model = new(modelDto.Id, "Test", 100, DateTime.Now, accountId, Guid.NewGuid());
            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.expense.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.monthlyBalances.Get(It.IsAny<Expression<Func<MonthlyBalances, bool>>>()));

            _unitOfWorkMock.Setup(u => u.accounts.Get(It.IsAny<Expression<Func<Accounts, bool>>>()))
                .ReturnsAsync(accounts);
            _unitOfWorkMock.Setup(u => u.expense.Get(It.IsAny<Expression<Func<Expense, bool>>>()))
                .ReturnsAsync(model);



            //Act 
            var result = await _expenseService.UpdateExpense(modelDto, user);

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
            ExpenseDto model = new() { Id = Guid.NewGuid(), Description = "Test", Amount = -100, Date = DateTime.Now, AccountId = accountId, CategoryId = Guid.NewGuid() };

            Accounts accounts = new Accounts(accountId, "test", 100, "test");
            _authServiceMock.Setup(s => s.CheckUserId(user))
               .Returns(Result.Success(userId));
            _unitOfWorkMock.Setup(u => u.expense.CheckUserAccess(model.Id, userId)).ReturnsAsync(true);

            //Act 
            var result = await _expenseService.UpdateExpense(model, user);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(GlobalError.InvalidAmount);
        }
    }
}
