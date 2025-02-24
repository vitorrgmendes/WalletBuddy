namespace WalletBuddy.Domain.Repositories;

public interface IUnitOfWork
{
    void Commit();
}
