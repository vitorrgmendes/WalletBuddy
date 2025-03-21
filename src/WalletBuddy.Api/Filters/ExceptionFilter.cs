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
        ShowExceptionLog(context);

        var errorResponse = new ResponseErrorJson("Unknow Server Error.");
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }

    private void ShowExceptionLog(ExceptionContext context)
    {
        var errorMessage = context.Exception?.Message ?? "An unknown error occurred.";
        var stackTrace = context.Exception?.StackTrace ?? "No stack trace available.";

        var logger = context.HttpContext.RequestServices.GetService<ILogger<ExceptionFilter>>();
        logger?.LogError("Unhandled exception occurred: {ErrorMessage}. Stack Trace: {StackTrace}. Request Path: {RequestPath}",
            errorMessage,
            stackTrace,
            context.HttpContext.Request.Path);
    }
}
