using Bank.DTO;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repository;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>().Property(a => a.Status).HasConversion<string>().HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Account>().Property(a => a.IBAN).HasColumnType("nvarchar(34)").IsRequired(true);
        modelBuilder.Entity<Account>().HasIndex(a => a.IBAN).IsUnique(true);
        modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnType("money").IsRequired();
        modelBuilder.Entity<Account>().Property(a => a.IsDelete).IsRequired().HasColumnType("bit").HasDefaultValueSql("(0)");
        modelBuilder.Entity<Account>().Property(a => a.CreateDate).IsRequired().HasColumnType("date").HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<Account>().HasOne(a => a.Customer).WithMany(a => a.Accounts).HasForeignKey(a => a.CustomerId).IsRequired(true);
        modelBuilder.Entity<Account>().HasMany(a => a.Cards).WithOne(a => a.Account).IsRequired(false);
        modelBuilder.Entity<Account>().HasMany(a => a.Transactions).WithOne(a => a.Account).IsRequired(false);

        modelBuilder.Entity<Card>().Property(a => a.Owner).HasColumnType("nvarchar(100)").IsRequired();
        modelBuilder.Entity<Card>().Property(a => a.Type).HasConversion<string>().HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Card>().HasIndex(a => new { a.Number, a.Cvc }).IsUnique(true);
        modelBuilder.Entity<Card>().Property(a => a.Number).HasColumnType("nvarchar(20)").IsRequired();
        modelBuilder.Entity<Card>().Property(a => a.Cvc).HasColumnType("nvarchar(4)").IsRequired();
        modelBuilder.Entity<Card>().Property(a => a.ExpirationDate).HasColumnType("date").IsRequired();
        modelBuilder.Entity<Card>().Property(a => a.CreateDate).HasDefaultValueSql("GetDate()").HasColumnType("date").IsRequired();
        modelBuilder.Entity<Card>().Property(a => a.IsDelete).HasColumnType("bit").HasDefaultValueSql("(0)").IsRequired();
        modelBuilder.Entity<Card>().HasOne(a => a.Account).WithMany(a => a.Cards).HasForeignKey(a => a.AccountId).IsRequired(false);
        modelBuilder.Entity<Card>().HasMany(a => a.Transactions).WithOne(a => a.Card).IsRequired(false);

        modelBuilder.Entity<City>().Property(a => a.Name).HasColumnType("nvarchar(20)").IsRequired();
        modelBuilder.Entity<City>().HasIndex(a => a.Name).IsUnique(true);
        modelBuilder.Entity<City>().Property(a => a.IsDelete).HasColumnType("bit").HasDefaultValueSql("(0)");
        modelBuilder.Entity<City>().Property(a => a.CreateDate).HasColumnType("date").HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<City>().HasMany(c => c.Customers).WithOne(c => c.City).IsRequired(false);

        modelBuilder.Entity<Customer>().Property(a => a.FirstName).HasColumnType("nvarchar(25)").IsRequired();
        modelBuilder.Entity<Customer>().Property(a => a.LastName).HasColumnType("nvarchar(25)").IsRequired();
        modelBuilder.Entity<Customer>().Property(a => a.Gender).HasConversion<string>().HasMaxLength(10);
        modelBuilder.Entity<Customer>().Property(a => a.Address).HasColumnType("nvarchar(75)").IsRequired();
        modelBuilder.Entity<Customer>().Property(a => a.Phone1).HasColumnType("nvarchar(15)").IsRequired(false);
        modelBuilder.Entity<Customer>().Property(a => a.Phone2).HasColumnType("nvarchar(15)").IsRequired(false);
        modelBuilder.Entity<Customer>().Property(a => a.PersonalNumber).HasColumnType("nvarchar(9)").IsRequired();
        modelBuilder.Entity<Customer>().Property(a => a.CreateDate).HasColumnType("date").HasDefaultValueSql("GetDate()").IsRequired();
        modelBuilder.Entity<Customer>().Property(a => a.IsDelete).HasColumnType("bit").HasDefaultValueSql("(0)").IsRequired();
        modelBuilder.Entity<Customer>().HasOne(u => u.User).WithOne(c => c.Customer).HasForeignKey<Customer>(u => u.UserId).IsRequired(false);
        modelBuilder.Entity<Customer>().HasOne(c => c.City).WithMany(c => c.Customers).HasForeignKey(c => c.CityId).IsRequired(false);
        modelBuilder.Entity<Customer>().HasMany(a => a.Accounts).WithOne(c => c.Customer).IsRequired(false);

        modelBuilder.Entity<Transaction>().Property(a => a.Amount).HasColumnType("money").IsRequired();
        modelBuilder.Entity<Transaction>().Property(a => a.Type).HasConversion<string>().HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Transaction>().Property(a => a.Date).HasColumnType("datetime2(7)").IsRequired();
        modelBuilder.Entity<Transaction>().Property(a => a.SourceAccount).HasColumnType("nvarchar(50)").IsRequired();
        modelBuilder.Entity<Transaction>().Property(a => a.DestinationAccount).HasColumnType("nvarchar(50)").IsRequired();
        modelBuilder.Entity<Transaction>().Property(a => a.IsDelete).HasColumnType("bit").HasDefaultValueSql("(0)");
        modelBuilder.Entity<Transaction>().Property(a => a.CreateDate).HasColumnType("date").HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<Transaction>().HasOne(a => a.Account).WithMany(a => a.Transactions).HasForeignKey(a => a.AccountId).IsRequired(false);
        modelBuilder.Entity<Transaction>().HasOne(a => a.Card).WithMany(a => a.Transactions).HasForeignKey(a => a.CardId).IsRequired(false);

        modelBuilder.Entity<User>().Property(a => a.UserName).HasColumnType("nvarchar(30)").IsRequired();
        modelBuilder.Entity<User>().HasIndex(a => a.UserName).IsUnique(true);
        modelBuilder.Entity<User>().Property(a => a.Password).HasColumnType("nvarchar(128)").IsRequired();
        modelBuilder.Entity<User>().Property(a => a.RegistrationDate).HasColumnType("datetime2(7)").IsRequired();
        modelBuilder.Entity<User>().Property(a => a.IsDelete).HasColumnType("bit").HasDefaultValueSql("(0)");
        modelBuilder.Entity<User>().Property(a => a.CreateDate).HasColumnType("date").HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<User>().HasOne(a => a.Customer).WithOne(a => a.User).IsRequired(false);
    }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Card> CreditCards { get; set; }

    public DbSet<City> Cities { get; set; }

}