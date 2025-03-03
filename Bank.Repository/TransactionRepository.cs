using Bank.DTO;
using Bank.Service.Interfaces.Repository;


namespace Bank.Repository;

internal sealed class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
	internal TransactionRepository(BankDbContext context) : base(context)
    {

    }
}
