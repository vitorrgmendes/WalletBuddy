using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Api.Filters.CustomAttributes;
using WalletBuddy.Application.Services.Users.ChangePassword;
using WalletBuddy.Application.Services.Users.GetProfile;
using WalletBuddy.Application.Services.Users.HardDelete;
using WalletBuddy.Application.Services.Users.Register;
using WalletBuddy.Application.Services.Users.Restore;
using WalletBuddy.Application.Services.Users.SoftDelete;
using WalletBuddy.Application.Services.Users.Update;
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
    public async Task<IActionResult> RegisterUser(
        [FromServices] IRegisterUser service,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await service.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpGet("Profile")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]    
    public async Task<IActionResult> GetUserProfile([FromServices] IGetUserProfile service)
    {
        var response = await service.Execute();
        return Ok(response);
    }

    [HttpPut("Profile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserProfile(
        [FromServices] IUpdateUser service,
        [FromBody] RequestUpdateUserJson request)
    {
        await service.Execute(request);
        return NoContent();
    }

    [HttpPut("Change-Password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePassword service,
        [FromBody] RequestChangePasswordJson request)
    {
        await service.Execute(request);
        return NoContent();
    }

    [HttpDelete("Delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> HardDelete(
        [FromServices] IHardDeleteUser service)
    {
        await service.Execute();
        return NoContent();
    }

    [HttpDelete("Soft-Delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SoftDelete(
        [FromServices] ISoftDeleteUser service)
    {
        await service.Execute();
        return NoContent();
    }

    [HttpPut("Restore/{id}")]
    [ApiKey]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RestoreUser(
        [FromServices] IRestoreUser service,
        [FromRoute] long id)
    {
        await service.Execute(id);
        return NoContent();
    }
}
