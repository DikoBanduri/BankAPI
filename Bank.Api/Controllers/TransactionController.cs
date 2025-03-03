using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public Task<IQueryable<Transaction>> GetTransactions()
    {
        var transaction = _transactionService.GetTransactions();
        return transaction ?? throw new ArgumentNullException($"No Transactions can be loaded!{nameof(transaction)}");
    }

    [HttpPost("make-transfer")]
    public async Task<IActionResult> MakeTransfer([FromBody] TransactionModel transferRequest)
    {
        if (transferRequest == null)
            return BadRequest("Invalid transfer data");

        try
        {
            var success = await _transactionService.MakeTransfer(
                transferRequest.SourceAccount,
                transferRequest.DestinationAccount,
                transferRequest.Amount
            );

            if (success)
                return Ok("Transfer successful");
            else
                return BadRequest("Transfer failed");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
