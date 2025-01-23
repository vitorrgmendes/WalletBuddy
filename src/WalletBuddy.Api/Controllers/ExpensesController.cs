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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Create([FromBody] RequestExpenseCreateJson request)
    {
        try
        {
            var service = new CreateExpense();
            var response = service.Execute(request);

            return Created(string.Empty, response);
        }
        catch (ArgumentException ex)
        {
            var errorResponse = new ResponseErrorJson(StatusCodes.Status400BadRequest, ex.Message);

            return BadRequest(errorResponse);            
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
