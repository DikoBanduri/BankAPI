using Bank.DTO;

namespace Bank.Model;

public record CardModel(string Owner, CardType Type, string Number, string Cvc, DateTime ExpirationDate, int? AccountId);


