using System.Text.Json.Serialization;

namespace Bank.DTO;

public class Card
{
    public int Id { get; set; }
    public string Owner { get; set; } = null!;
    public CardType Type { get; set; }
    public string Number { get; set; } = null!;
    public string Cvc { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
    public int? AccountId { get; set; }
    public Account? Account { get; set; } = null!;
    public ICollection<Transaction>? Transactions { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CardType
{
    MasterCard,
    Visa
}