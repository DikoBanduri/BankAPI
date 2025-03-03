using System.Text.Json.Serialization;

namespace Bank.DTO;

public class Account
{
	public int Id { get; set; }
	public AccountStatus Status { get; set; }
	public string IBAN { get; set; } = null!;
	public decimal Balance { get; set; }
	public DateTime CreateDate { get; set; }
	public bool IsDelete { get; set; }
	public int? CustomerId { get; set; }
	public Customer? Customer { get; set; } = null!;
	public ICollection<Card>? Cards { get; set; }
	public ICollection<Transaction>? Transactions { get; set; }

	Random rand = new();
    public Account()
    {
		IBAN = Convert.ToString((long) rand.NextDouble() * 9_000_000_000L + 1_000_000_000L);
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountStatus
{
    Ongoing,
    Suspended
}