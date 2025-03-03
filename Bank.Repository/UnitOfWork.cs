using Bank.Service.Interfaces.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bank.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _context;
    private readonly Lazy<IAccountRepository> _accountRepository;
    private readonly Lazy<ICustomerRepository> _customerRepository;
    private readonly Lazy<ICreditCardRepository> _creditCardRepository;
    private readonly Lazy<ITransactionRepository> _transactionRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ICityRepository> _cityRepository;

    public UnitOfWork(BankDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
        _customerRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(context));
        _creditCardRepository = new Lazy<ICreditCardRepository>(() => new CreditCardRepository(context));
        _transactionRepository = new Lazy<ITransactionRepository>(() => new TransactionRepository(context));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(context));
        _cityRepository = new Lazy<ICityRepository>(() => new CityRepository(context));
    }

    public IAccountRepository AccountRepository => _accountRepository.Value;

    public ICustomerRepository CustomerRepository => _customerRepository.Value;

    public ICreditCardRepository CreditCardRepository => _creditCardRepository.Value;

    public ITransactionRepository TransactionRepository => _transactionRepository.Value;

    public IUserRepository UserRepository => _userRepository.Value;

    public ICityRepository CityRepository => _cityRepository.Value;

    public int SaveChanges() => _context.SaveChanges();
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void BeginTransaction()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            throw new InvalidOperationException("A Transaction is already in progress.");
        }

        _context.Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            throw new InvalidOperationException("A Transaction is already in progress.");
        }
        return await _context.Database.BeginTransactionAsync();
    }

    public void CommitTransaction()
    {
        try
        {
            _context.Database.CurrentTransaction?.Commit();
        }
        catch
        {
            _context.Database.CurrentTransaction?.Rollback();
            throw;
        }
        finally
        {
            _context.Database.CurrentTransaction?.Dispose();
        }
    }

    public async Task CommitTransactionAsync()
    {
        var transaction = _context.Database.CurrentTransaction;

        if (transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public void RollBack()
    {
        try
        {
            _context.Database.CurrentTransaction?.Rollback();
        }
        finally
        {
            _context.Database.CurrentTransaction?.Dispose();
        }
    }

    public async Task RollBackAsync()
    {
        var transaction = _context.Database.CurrentTransaction;
        if (transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await transaction.RollbackAsync();
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}

