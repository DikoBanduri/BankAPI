using Bank.DTO;
using Bank.Service.Interfaces.Repository;

namespace Bank.Repository;

internal sealed class CreditCardRepository : RepositoryBase<Card>, ICreditCardRepository
{
    internal CreditCardRepository(BankDbContext context) : base(context)
    {

    }
}
