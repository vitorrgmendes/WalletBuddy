namespace WalletBuddy.Communication.Responses.Error;

public class ResponseErrorJson
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public ResponseErrorJson(int statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}
