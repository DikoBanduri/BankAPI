using Bank.DTO;

namespace Bank.Service.Interfaces.Services;

public interface IUserService
{
    Task<User> GetUser(int id); 
    Task<IQueryable<User>> GetUsers(); 
    void CreateUser(User user);
    void UpdateUser(User user); 
    void ResetPassword(string userName, string oldPassword, string newPassword);
    void DeleteUser(int userId);

    Task<User> GetUserAsync(int userId);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}
