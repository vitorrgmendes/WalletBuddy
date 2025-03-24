using WalletBuddy.Domain.Security.Tokens;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }

    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authorization))
            throw new InvalidCredentialsException();

        return authorization["Bearer ".Length..].Trim();
    }
}
