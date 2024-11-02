

namespace SpendingTracker.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        IAuthRepository auth { get; }
        IAccountRepository accounts { get; }
        IUserAccountRepository userAccounts { get; }
        Task Save();
    }
}
