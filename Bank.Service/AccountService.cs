using Bank.DTO;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<Account> GetAccount(int accountId)
    {
        try
        {
            Account account = _unitOfWork.AccountRepository.Get(accountId);
            return Task.FromResult(account);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException("The AccountId could not be found", ex);
        }
    }

    public Task<IQueryable<Account>> GetAccounts()
    {
        var accounts = _unitOfWork.AccountRepository.Set();
        if (accounts == null)
        {
            throw new InvalidDataException("The AccountId could not be found");
        }

        return Task.FromResult(accounts);
    }

    public void CreateAccount(Account account)
    {
        if (account == null) throw new ArgumentNullException(nameof(account));

        _unitOfWork.AccountRepository.Insert(account);
        _unitOfWork.SaveChanges();
    }

    public void UpdateAccount(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        _unitOfWork.AccountRepository.Update(account);
        _unitOfWork.SaveChanges();
    }

    public void SuspendAccount(int accountId)
    {
        Account account = _unitOfWork.AccountRepository.Get(accountId);

        if (account.Status == AccountStatus.Suspended) throw new ArgumentException($"The {nameof(account)} is already Suspended");

        account.Status = AccountStatus.Suspended;
        _unitOfWork.SaveChanges();
    }

    public void ResumeAccount(int accountId)
    {
        Account account = _unitOfWork.AccountRepository.Get(accountId);
        if (account.Status == AccountStatus.Ongoing) throw new ArgumentException($"The {nameof(account)} is already Ongoing");

        account.Status = AccountStatus.Ongoing;
        _unitOfWork.SaveChanges();
    }

    public void DeleteAccount(int accountId)
    {
        Account account = _unitOfWork.AccountRepository.Get(accountId) ?? throw new ArgumentNullException($"The {accountId} does not exist.");
        account.IsDelete = true;
        _unitOfWork.AccountRepository.Update(account);
        _unitOfWork.SaveChanges();
    }

    public async Task<Account> GetAccountAsync(int accountId)
    {
        try
        {
            return await _unitOfWork.AccountRepository.GetAsync(accountId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException("The AccountId could not be found", ex);
        }
    }

    public async Task CreateAccountAsync(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        await _unitOfWork.AccountRepository.InsertAsync(account);
       await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        _unitOfWork.AccountRepository.Update(account);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int accountId)
    {
        Account account = await _unitOfWork.AccountRepository.GetAsync(accountId) ?? throw new ArgumentNullException($"The {accountId} does not exist.");
        account.IsDelete = true;
        await _unitOfWork.AccountRepository.InsertOrUpdateAsync(account);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task SuspendAccountAsync(int accountId)
    {
        Account account = await _unitOfWork.AccountRepository.GetAsync(accountId);

        if (account.Status == AccountStatus.Suspended) throw new ArgumentException($"The {nameof(account)} is already Suspended");

        account.Status = AccountStatus.Suspended;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ResumeAccountAsync(int accountId)
    {
        Account account =await _unitOfWork.AccountRepository.GetAsync(accountId);
        if (account.Status == AccountStatus.Ongoing) throw new ArgumentException($"The {nameof(account)} is already Ongoing");

        account.Status = AccountStatus.Ongoing;
        await _unitOfWork.SaveChangesAsync();
    }
}
