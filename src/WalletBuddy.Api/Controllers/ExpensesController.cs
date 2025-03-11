using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Application.Services.Expenses.Delete;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Application.Services.Expenses.GetById;
using WalletBuddy.Application.Services.Expenses.Update;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseExpenseCreatedJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateExpense service,
        [FromBody] RequestExpenseJson request)
    {
        var response = await service.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpenses service)
    {
        var response = await service.Execute();

        if (response.Expenses.Count > 0)        
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetExpenseById([FromServices] IGetExpenseById service, [FromRoute] long id)
    {
        var response = await service.Execute(id);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromServices] IDeleteExpense service, [FromRoute] long id)
    {
        await service.Execute(id);
        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateExpense service,
        [FromBody] RequestExpenseJson request,
        [FromRoute] long id)
    {
        await service.Execute(id, request);
        return NoContent();
    }
}
