using Bank.DTO;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IQueryable<Transaction>> GetTransactions()
    {
        var transactions = _unitOfWork.TransactionRepository.Set();
        return Task.FromResult(transactions) ?? throw new InvalidDataException("The cardId couldn't not be found in within the data.");
    }

    public async Task<Transaction> GetTransactionAsync(int transactionId)
    {
        try
        {
            return await _unitOfWork.TransactionRepository.GetAsync(transactionId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException($"The TransactionId couldn't not be found in within the data {ex}.");
        }
    }

    public async Task<bool> MakeTransfer(int fromAccount, int toAccount, decimal amount)
    {
        var sourceAccount = await _unitOfWork.AccountRepository.GetAsync(fromAccount);
        var destinationAccount = await _unitOfWork.AccountRepository.GetAsync(toAccount);

        if (sourceAccount == null || destinationAccount == null)
            throw new InvalidOperationException("One or both accounts not found.");

        if (sourceAccount.Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        // Update the balances
        sourceAccount.Balance -= amount;
        destinationAccount.Balance += amount;

        // Attach the accounts and mark them as modified
        _unitOfWork.AccountRepository.Update(sourceAccount);
        _unitOfWork.AccountRepository.Update(destinationAccount);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}