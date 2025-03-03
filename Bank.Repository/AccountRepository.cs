using Bank.DTO;
using Bank.Service.Interfaces.Repository;

namespace Bank.Repository;

internal sealed class AccountRepository : RepositoryBase<Account>, IAccountRepository
{
	internal AccountRepository(BankDbContext context) : base(context)
    {

    }
}
