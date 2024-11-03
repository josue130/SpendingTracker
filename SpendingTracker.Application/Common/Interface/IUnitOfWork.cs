

namespace SpendingTracker.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        IAuthRepository auth { get; }
        IAccountRepository accounts { get; }
        IUserAccountRepository userAccounts { get; }
        ICategoryIncomeRepository categoryIncome { get; }
        IIncomeRepository income { get; }
        Task Save();
    }
}
