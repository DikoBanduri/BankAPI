using Bank.DTO;
using Bank.Service.Interfaces.Repository;

namespace Bank.Repository;

internal sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
	internal UserRepository(BankDbContext context) : base(context)
    {

    }
}
