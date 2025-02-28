namespace WalletBuddy.Exception.Exception;
public abstract class WalletBuddyException : SystemException
{
    protected WalletBuddyException(string message) : base(message)
    { }
}