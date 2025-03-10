using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Login;
using WalletBuddy.Communication.Requests.Login;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
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
}
