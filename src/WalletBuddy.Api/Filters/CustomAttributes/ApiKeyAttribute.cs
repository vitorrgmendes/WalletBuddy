using Microsoft.AspNetCore.Mvc;

namespace WalletBuddy.Api.Filters.CustomAttributes;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute() : base(typeof(ApiKeyAuthFilter))
    {
    }
}
