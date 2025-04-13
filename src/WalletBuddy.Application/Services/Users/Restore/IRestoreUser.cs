namespace WalletBuddy.Application.Services.Users.Restore;

public interface IRestoreUser
{
    Task Execute(long id);
}
