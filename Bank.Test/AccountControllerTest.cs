using Bank.Api.Controllers;
using Bank.DTO;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Bank.Test;
public class AccountControllerTest
{
    private readonly ITestOutputHelper _testOutput;
    private readonly Mock<IAccountService> _accountServiceMock;
    private readonly Mock<ILogger<AccountController>> _loggerMock;
    private readonly AccountController _accountController;

    public AccountControllerTest(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
        _accountServiceMock = new Mock<IAccountService>();
        _loggerMock = new Mock<ILogger<AccountController>>();

        _accountController = new AccountController(_accountServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAccount()
    {
        var accountId = 1;
        var mockAccount = new Account { Id = accountId, Status = (AccountStatus)1, Balance = 1000 };
        _accountServiceMock.Setup(s => s.GetAccount(accountId))
        .ReturnsAsync(mockAccount);
        _accountServiceMock.Setup(service => service.GetAccount(accountId))
                          .ThrowsAsync(new InvalidDataException("The AccountId could not be found"));

        var result = await _accountController.GetAccount(accountId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnAccount = Assert.IsType<Account>(okResult.Value);
        Assert.Equal(accountId, returnAccount.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("The AccountId could not be found", notFoundResult.Value);

      
        _testOutput.WriteLine("Test Passed: Account not found as expected.");
    }
}
