using Bank.DTO;

namespace Bank.Service.Interfaces.Services;

public interface IAccountService
{
    Task<Account> GetAccount(int id);
    Task<IQueryable<Account>> GetAccounts();
    void CreateAccount(Account account);
    void UpdateAccount(Account account);
    void SuspendAccount(int accountId);
    void ResumeAccount(int accountId);
    void DeleteAccount(int accountId);

    Task<Account> GetAccountAsync(int accountId);
    Task CreateAccountAsync(Account account);
    Task UpdateAccountAsync(Account account);
    Task SuspendAccountAsync(int accountId);
    Task ResumeAccountAsync(int accountId);
    Task DeleteAccountAsync(int accountId);
}