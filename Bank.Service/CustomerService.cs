using Bank.DTO;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public Task<Customer> GetCustomer(int customerId)
    {
        Customer customer = _unitOfWork.CustomerRepository.Get(customerId);
        return Task.FromResult(customer) ?? throw new InvalidDataException("CustomerId could not be found within data.");
    }

    public Task<IQueryable<Customer>> GetCustomers()
    {
        var customers = _unitOfWork.CustomerRepository.Set();
        return Task.FromResult(customers) ?? throw new InvalidDataException("CustomerId could not be found within data.");
    }

    public void AddCustomer(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException("The customer needs to be provided.");
        _unitOfWork.CustomerRepository.Insert(customer);
        _unitOfWork.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException("The customer needs to be provided.");
        _unitOfWork.CustomerRepository.Update(customer);
        _unitOfWork.SaveChanges();
    }

    public void DeleteCustomer(int customerId)
    {
        Customer customer = _unitOfWork.CustomerRepository.Get(customerId) ?? throw new ArgumentNullException($"The {customerId} does not exist.");
        customer.IsDelete = true;
        _unitOfWork.CustomerRepository.Update(customer);
        _unitOfWork.SaveChanges();
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        try
        {
            return await _unitOfWork.CustomerRepository.GetAsync(customerId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException($"CustomerId could not be found within data {ex}.");
        }
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException("The customer needs to be provided.");
        await _unitOfWork.CustomerRepository.InsertAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException("The customer needs to be provided.");
        _unitOfWork.CustomerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        Customer customer = await _unitOfWork.CustomerRepository.GetAsync(customerId) ?? throw new ArgumentNullException($"The {customerId} does not exist.");
        customer.IsDelete = true;
        await _unitOfWork.CustomerRepository.InsertOrUpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }
}
