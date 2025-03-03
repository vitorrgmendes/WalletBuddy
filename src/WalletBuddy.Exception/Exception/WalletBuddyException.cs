namespace WalletBuddy.Exception.Exception;
public abstract class WalletBuddyException : SystemException
{
    protected WalletBuddyException(string message) : base(message)
    { 
    
    }

    public abstract int StatusCode { get; }

    public abstract List<string> GetErrors();
}