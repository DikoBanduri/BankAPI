using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public Task<IQueryable<User>> GetUsers()
    {
        var users = _userService.GetUsers();
        return users ?? throw new ArgumentNullException($"The items could not be found.");
    }

    [HttpGet("{id:int}")]
    public async Task<User> GetUserAsync(int id)
    {
        var user = await _userService.GetUser(id);
        return user ?? throw new ArgumentNullException($"The item with given Id: {nameof(id)} could not be found.");
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUserAsync(UserModel userModel)
    {
        User user = new()
        {
            UserName = userModel.UserName,
            Password = userModel.Password
        };

        await _userService.CreateUserAsync(user);
        return NoContent();
    }

    [HttpPut("{id:int}/update")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel userModel)
    {
        if (userModel == null)
            return BadRequest("Invalid user data");

        var existingUser = await _userService.GetUserAsync(id);
        if (existingUser == null)
            return NotFound($"User with ID {id} not found.");

        existingUser.UserName = userModel.UserName;

        _userService.UpdateUser(existingUser);

        return NoContent();
    }

    [HttpPut("{id:int}/reset")]
    public async Task<IActionResult> ResetPassword(int id, [FromBody] UserModel model, string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
            return BadRequest("New password cannot be empty.");

        var existingUser = await _userService.GetUser(id);
        if (existingUser == null)
            return NotFound($"User with ID {id} not found.");

        _userService.ResetPassword(existingUser.UserName, model.Password, newPassword);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound($"User with ID {id} not found");

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}