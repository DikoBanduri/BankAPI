using Bank.DTO;
using Bank.Service.Interfaces.Repository;

namespace Bank.Repository;

internal sealed class CityRepository : RepositoryBase<City>, ICityRepository
{
    internal CityRepository(BankDbContext context) : base(context)
    {

    }
}

