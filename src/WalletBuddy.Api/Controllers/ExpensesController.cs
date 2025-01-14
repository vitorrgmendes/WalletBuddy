using Microsoft.AspNetCore.Mvc;
using WalletBuddy.Communication.Requests.Expenses;

namespace WalletBuddy.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] RequestExpenseCreateJson request)
    { 
        return Created();
    }
}
