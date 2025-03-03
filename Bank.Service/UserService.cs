using Bank.DTO;
using Bank.Extension;
using Bank.Service.Interfaces.Repository;
using Bank.Service.Interfaces.Services;

namespace Bank.Service;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<User> GetUser(int userId)
    {
        User user = _unitOfWork.UserRepository.Get(userId);
        if (user != null)
        {
            return Task.FromResult(user);
        }
        else
        {
            throw new InvalidDataException("The UserId could not be found");
        }
    }

    public Task<IQueryable<User>> GetUsers()
    {
        var users = _unitOfWork.UserRepository.Set();
        if (users != null)
        {
            return Task.FromResult(users);
        }
        else
        {
            throw new InvalidDataException("The UserId could not be found");
        }
    }

    public void CreateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        user.Password = StringExtensions.GetHashString(user.Password);
        _unitOfWork.UserRepository.Insert(user);
        SaveChanges();
    }

    public void UpdateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.SaveChanges();
    }

    public void ResetPassword(string userName, string oldPassword, string newPassword)
    {
        var user = _unitOfWork.UserRepository.Set().SingleOrDefault(u => u.UserName == userName && u.Password == oldPassword);
        if (user != null)
        {
            user.Password = newPassword;
            _unitOfWork.UserRepository.Update(user);
            SaveChanges();
        }
        else
        {
            throw new InvalidDataException("User not found");
        }
    }

    public void DeleteUser(int id)
    {
        User user = _unitOfWork.UserRepository.Get(id) ?? throw new ArgumentNullException($"The user with Id: {id} does not exist.");
        user.IsDelete = true;
        _unitOfWork.UserRepository.Update(user);
        SaveChanges();
    }

    public bool Login(string username, string password)
    {
        if (username == null) throw new ArgumentNullException(nameof(username));
        if (password == null) throw new ArgumentNullException(nameof(password));

        return _unitOfWork
            .UserRepository
            .Set(u => u.UserName == username && u.Password == password && !u.IsDelete)
            .SingleOrDefault() != default;
    }

    public void SaveChanges()
    {
        _unitOfWork.SaveChanges();
    }

    public async Task<User> GetUserAsync(int userId)
    {
        try
        {
            return await _unitOfWork.UserRepository.GetAsync(userId);
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidDataException($"UserId could not be found within data {ex}.");
        }
    }

    public async Task CreateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException("The User needs to be provided");

        await _unitOfWork.UserRepository.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException("The User needs to be provided");

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        User user = await _unitOfWork.UserRepository.GetAsync(userId) ?? throw new InvalidDataException($"The {userId} doesn't exist!");
        user.IsDelete = true;
        await _unitOfWork.UserRepository.InsertOrUpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
