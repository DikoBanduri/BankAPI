using Bank.DTO;

namespace Bank.Model;

public record TransactionModel(decimal Amount, TransactionType Type, int SourceAccount, int DestinationAccount, int AccountId, int CardId);
