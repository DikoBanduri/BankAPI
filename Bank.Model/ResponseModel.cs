namespace Bank.Model;

public record ResponseModel<T>(string ResponseCode, string ResponseMessage, T? Data)
{
    public string RequestId { get; } = Guid.NewGuid().ToString();
}
