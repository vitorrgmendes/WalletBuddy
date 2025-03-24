using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Users.GetProfile;
using WalletBuddy.Application.Services.Users.Register;
using WalletBuddy.Communication.Requests.Users;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Users;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
// [ApiKey]
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

    [HttpGet("Profile")]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfile service)
    {
        var response = await service.Execute();
        return Ok(response);
    }

}
