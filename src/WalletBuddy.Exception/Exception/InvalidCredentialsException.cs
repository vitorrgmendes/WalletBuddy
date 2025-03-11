
using System.Net;

namespace WalletBuddy.Exception.Exception;

public class InvalidCredentialsException : WalletBuddyException
{
    public InvalidCredentialsException() : base(ResourceErrorMessages.INVALID_CREDENTIALS)
    {        
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
