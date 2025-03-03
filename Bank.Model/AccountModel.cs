using Bank.DTO;

namespace Bank.Model;

public record AccountModel(AccountStatus AccountStatus, decimal Balance, string IBAN, int? CustomerId);

