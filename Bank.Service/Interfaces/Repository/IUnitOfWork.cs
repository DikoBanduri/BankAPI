
using Microsoft.EntityFrameworkCore.Storage;

namespace Bank.Service.Interfaces.Repository;

public interface IUnitOfWork : IDisposable
{
    IAccountRepository AccountRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    ICreditCardRepository CreditCardRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    IUserRepository UserRepository { get; }
    ICityRepository CityRepository { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
    void BeginTransaction();
    void CommitTransaction();
    void RollBack();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollBackAsync();
}
