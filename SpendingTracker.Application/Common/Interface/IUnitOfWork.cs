

namespace SpendingTracker.Application.Common.Interface
{
    public interface IUnitOfWork
    {
        IAuthRepository auth { get; }
        Task Save();
    }
}
