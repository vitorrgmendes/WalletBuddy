using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WalletBuddy.Application.Services.Expenses.Reports.Excel;
using WalletBuddy.Communication.Responses.Error;

namespace WalletBuddy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExcel([FromServices] IGenerateExpensesReportExcel service, [FromHeader] DateOnly date)
        {
            var file = await service.Execute(date);

            if (file.Length > 0)
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

            return NoContent();
        }
    }
}
