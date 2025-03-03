using Bank.DTO;
using Bank.Service.Interfaces.Repository;

namespace Bank.Repository;

internal sealed class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
	internal CustomerRepository(BankDbContext context) : base(context)
    {

    }
}
