using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [HttpGet]
    public Task<IQueryable<Customer>> GetCustomers()
    {
        var customers = _customerService.GetCustomers();
        return customers ?? throw new ArgumentNullException($"No Customers can be loaded!{nameof(customers)}");
    }

    [HttpGet("{id:int}")]
    public async Task<Customer> GetCustomerAsync(int id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        return customer ?? throw new ArgumentNullException(nameof(id));
    }


    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerModel customerModel)
    {
        if (customerModel == null)
            return BadRequest("Invalid customer data.");

        Customer customer = new()
        {
            FirstName = customerModel.FirstName,
            LastName = customerModel.LastName,
            Gender = customerModel.Gender,
            Address = customerModel.Address,
            PersonalNumber = customerModel.PersonalNumber,
            CityId = customerModel.CityId,
            UserId = customerModel.UserId
        };

        await _customerService.AddCustomerAsync(customer);
        return Ok(customer);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerModel customerModel)
    {
        if (customerModel == null)
            return BadRequest("Invalid customer data.");

        var existingCustomer = await _customerService.GetCustomer(id);
        if (existingCustomer == null)
            return NotFound($"Customer with ID {id} not found.");

        existingCustomer.FirstName = customerModel.FirstName;
        existingCustomer.LastName = customerModel.LastName;
        existingCustomer.Gender = customerModel.Gender;
        existingCustomer.Address = customerModel.Address;
        existingCustomer.PersonalNumber = customerModel.PersonalNumber;
        existingCustomer.CityId = customerModel.CityId;
        existingCustomer.UserId = customerModel.UserId;

        _customerService.UpdateCustomer(existingCustomer);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomerAsync(int id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null)
            return NotFound($"Customer with ID {id} not found.");

        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
