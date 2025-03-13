using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Users.Register;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("Register")]
    [ProducesResponseType(typeof(ResponseUserRegisteredJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser(
        [FromServices] IRegisterUser service,
        [FromBody] RequestUserJson request)
    {
        var response = await service.Execute(request);
        return Created(string.Empty, response);
    }
}
