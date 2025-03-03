namespace Bank.DTO;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime RegistrationDate { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }
    public Customer? Customer { get; set; } = null!;
}