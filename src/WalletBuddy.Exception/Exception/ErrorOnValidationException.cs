namespace WalletBuddy.Exception.Exception;

public class ErrorOnValidationException : WalletBuddyException
{
    public List<string> Errors { get; set; }

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        Errors = errorMessages;
    }
}