using Bank.DTO;

namespace Bank.Service.Interfaces.Services;

public interface ICustomerService
{
    Task<Customer> GetCustomer(int customerId);
    Task<IQueryable<Customer>> GetCustomers();
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);

    Task<Customer> GetCustomerAsync(int customerId);
    Task AddCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int customerId);
}
