using Bank.DTO;

namespace Bank.Service.Interfaces.Services;

public interface ITransactionService
{
    Task<IQueryable<Transaction>> GetTransactions();
    Task<Transaction> GetTransactionAsync(int transactionId);
    Task<bool> MakeTransfer(int fromAccount, int toAccount, decimal amount);
}
