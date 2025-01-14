using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Application.Services.Expenses;
using WalletBuddy.Communication.Requests.Expenses;
using WalletBuddy.Communication.Responses.Expenses;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseExpenseCreatedJson), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] RequestExpenseCreateJson request)
    {
        var service = new CreateExpense();
        var response = service.Execute(request);        

        return Created(string.Empty, response);
    }
}
