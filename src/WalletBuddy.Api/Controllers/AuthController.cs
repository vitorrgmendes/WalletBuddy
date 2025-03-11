using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Auth.Login;
using WalletBuddy.Application.Services.Auth.RefreshToken;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("Login")]
    [ProducesResponseType(typeof(ResponseUserRegisteredJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUser service,
        [FromBody] RequestLoginJson request)
    {
        var response = await service.Execute(request);

        return Ok(response);
    }

    [HttpPost("RefreshToken")]
    [ProducesResponseType(typeof(ResponseUserRegisteredJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken(
        [FromServices] IRefreshToken service,
        [FromBody] RequestRefreshTokenJson request)
    {
        var response = await service.Execute(request);

        return Ok(response);
    }
}
