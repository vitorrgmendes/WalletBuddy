using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Expenses.Create;
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
    public IActionResult Create(
        [FromServices] ICreateExpense service,
        [FromBody] RequestExpenseCreateJson request)
    {
        var response = service.Execute(request);

        return Created(string.Empty, response);
    }
}
