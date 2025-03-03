using WalletBuddy.Domain.Entities;

namespace WalletBuddy.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);

    Task<List<Expense>> GetAll();

    Task<Expense?> GetById(long id);
    
    /// <summary>This function deletes an expense by id.</summary>
    /// <param name="id"></param>
    /// <returns>Returns TRUE if the deletion was successful, otherwise returns FALSE.</returns>
    Task<bool> DeleteById(long id);

    void Update(Expense expense);

    Task<Expense?> GetByIdForChanges(long id);
}
