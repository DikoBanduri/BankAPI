using System.Text.Json.Serialization;

namespace Bank.DTO;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public bool IsSuccessful => Status.Equals(TransactionStatus.Success);
    public DateTime Date { get; set; }
    public int SourceAccount { get; set; }  // Sender
    public int DestinationAccount { get; set; }  // Receiver
    public int AccountId { get; set; }
    public Account? Account { get; set; } = null!;
    public int? CardId { get; set; }
    public Card? Card { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionStatus
{
    Success,
    Failed,
    Error
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Transfer,
    Deposit,
    Withdrawal
}