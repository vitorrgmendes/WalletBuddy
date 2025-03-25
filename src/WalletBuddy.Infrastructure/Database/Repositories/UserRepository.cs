using Microsoft.EntityFrameworkCore;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Repositories.Users;

namespace WalletBuddy.Infrastructure.Database.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly WalletBuddyDbContext _dbContext;

    public UserRepository(WalletBuddyDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetUserById(long id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public async Task Register(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}
