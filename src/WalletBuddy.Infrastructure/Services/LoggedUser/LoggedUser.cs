using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using WalletBuddy.Domain.Entities;
using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Domain.Services.LoggedUser;
using WalletBuddy.Exception.Exception;
using WalletBuddy.Infrastructure.Database;

namespace WalletBuddy.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly WalletBuddyDbContext _dbContext;
    private readonly string _token;

    public LoggedUser(
        WalletBuddyDbContext dbContext, 
        ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _token = tokenProvider.TokenOnRequest();
    }

    public async Task<User> Get()
    {
        return await QueryUser(
            GetUserIdentifierFromToken(), 
            asNoTracking: true);
    }

    public async Task<User> GetForChanges()
    {
        return await QueryUser(
            GetUserIdentifierFromToken(), 
            asNoTracking: false);
    }

    private string GetUserIdentifierFromToken()
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(_token);

        return jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
    }

    private async Task<User> QueryUser(string userIdentifier, bool asNoTracking)
    {
        var query = _dbContext.Users;

        if (asNoTracking)
            query.AsNoTracking();

        var user = await query.FirstOrDefaultAsync(user => user.UserIdentifier == Guid.Parse(userIdentifier));

        return user is null ? throw new InvalidCredentialsException() : user;
    }
}
