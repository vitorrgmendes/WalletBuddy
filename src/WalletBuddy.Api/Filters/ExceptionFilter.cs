using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WalletBuddy.Communication.Responses.Error;
using WalletBuddy.Exception.Exception;

namespace WalletBuddy.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is WalletBuddyException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var walletBuddyException = (WalletBuddyException)context.Exception;
        var errorResponse = new ResponseErrorJson(walletBuddyException.GetErrors());

        context.HttpContext.Response.StatusCode = walletBuddyException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ThrowUnknowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(context.Exception.Message);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
