namespace Bank.Api.Controllers;

public class AddController
{
    public interface AddService { }
    public class AdsService : AddService { }
    public interface ILogger { }
    public class ConsoleLogger : ILogger { }
    public class FileLogger : ILogger { }
    public AddController(AddService adsService, ILogger logger) => this.Logger = logger;
    public ILogger Logger { get; }
}
