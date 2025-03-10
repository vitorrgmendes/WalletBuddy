
using System.Net;

namespace WalletBuddy.Exception.Exception;

public class InvalidLoginException : WalletBuddyException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_CREDENTIALS)
    {        
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
