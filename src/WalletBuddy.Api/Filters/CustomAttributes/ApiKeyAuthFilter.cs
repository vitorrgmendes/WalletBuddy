using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WalletBuddy.Domain.Security.ApiKey;
using WalletBuddy.Domain.Security.Constants;
using WalletBuddy.Exception;

namespace WalletBuddy.Api.Filters.CustomAttributes;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IApiKeyValidation _apiKeyValidation;

    public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
    {
        _apiKeyValidation = apiKeyValidation;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userApiKey = context.HttpContext.Request.Headers[SecurityConstants.API_KEY_HEADER_NAME];

        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            context.Result = new BadRequestObjectResult(new { Message = ResourceErrorMessages.MISSING_API_KEY });
            return;
        }

        if (!_apiKeyValidation.IsValidApiKey(userApiKey!))
        {
            context.Result = new UnauthorizedObjectResult(new { Message = ResourceErrorMessages.INVALID_API_KEY });
            return;
        }
    }
}
