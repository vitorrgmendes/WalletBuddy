namespace WalletBuddy.Application.Services.Expenses.Delete;

public interface IDeleteExpense
{
    Task Execute(long id);
}
