using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public Task<IQueryable<Account>> GetAccounts()
    {
        var accounts = _accountService.GetAccounts();
        return accounts ?? throw new ArgumentNullException($"The accounts could not be loaded{nameof(accounts)}");
    }

    [HttpGet("{id:int}")]
    public async Task<Account> GetAccount(int id)
    {
        var account = await _accountService.GetAccount(id);
        return account ?? throw new ArgumentNullException(nameof(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountAsync(AccountModel accountModel)
    {
        if (accountModel == null)
            return BadRequest("Invalid account data");

        Account account = new()
        {
            Status = accountModel.AccountStatus,
            Balance = accountModel.Balance,
            IBAN = accountModel.IBAN,
            CustomerId = accountModel.CustomerId
        };

        await _accountService.CreateAccountAsync(account);
        return Ok(account);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountModel accountModel)
    {
        if (accountModel == null)
            return BadRequest("Account model cannot be null.");

        var existingAccount = await _accountService.GetAccountAsync(id);

        if (existingAccount == null)
            return NotFound($"Account with ID {id} not found.");

        existingAccount.Status = accountModel.AccountStatus;
        existingAccount.Balance = accountModel.Balance;
        existingAccount.IBAN = accountModel.IBAN;
        existingAccount.CustomerId = accountModel.CustomerId;

        _accountService.UpdateAccount(existingAccount);

        return NoContent();
    }

    [HttpPut("{id:int}/suspended")]
    public async Task<IActionResult> SuspendAccount(int id)
    {
        var account = await _accountService.GetAccountAsync(id);

        if (account == null)
            return NotFound(new { Message = $"Account with ID {id} not found." });

        if (account.Status == AccountStatus.Suspended)
            return BadRequest(new { Message = "The account is already suspended." });

        await _accountService.SuspendAccountAsync(id);
        return NoContent();
    }

    [HttpPut("{id:int}/resume")]
    public async Task<IActionResult> ResumeAccount(int id)
    {
        var account = await _accountService.GetAccountAsync(id);

        if (account == null)
            return NotFound(new { Message = $"Account with ID {id} not found." });

        if (account.Status == AccountStatus.Ongoing)
            return BadRequest(new { Message = "The account is already active." });

        await _accountService.ResumeAccountAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAccountAsync(int id)
    {
        var account = await _accountService.GetAccountAsync(id);
        if (account == null)
            return NotFound($"Account with ID {id} not found");

        await _accountService.DeleteAccountAsync(id);
        return NoContent();
    }
}
