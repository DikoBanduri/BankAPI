using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bank.DTO;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string Address { get; set; } = null!;
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string PersonalNumber { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; } = null!;
    public int? CityId { get; set; }
    public City? City { get; set; } = null!;
    public ICollection<Account> Accounts { get; set; } = null!;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female
}