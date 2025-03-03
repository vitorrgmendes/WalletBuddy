using System.Net;

namespace WalletBuddy.Exception.Exception;

public class NotFoundException : WalletBuddyException
{
    public NotFoundException(string message) : base(message)
    { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
