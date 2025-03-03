using Bank.Repository;
using Bank.Service;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Configuration;

public static class DependencyConfiguration
{
    public static void ConfigureDependency(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICardService, CardService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlServer(connectionString));
    }
}