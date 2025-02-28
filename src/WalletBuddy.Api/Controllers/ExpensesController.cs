using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Expenses.Create;
using WalletBuddy.Application.Services.Expenses.GetAll;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseExpenseCreatedJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateExpense service,
        [FromBody] RequestExpenseCreateJson request)
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
}
