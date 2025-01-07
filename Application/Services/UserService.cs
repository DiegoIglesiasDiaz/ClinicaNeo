using Application.Interfaces;
using Infrastructure.Interfaces;
using Domain.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User CreateUser(string firstName, string lastName, string email)
    {
        var user = new User(firstName, lastName, email);
        _userRepository.Add(user);
        return user;
    }

    public void UpdateUser(Guid id, string firstName, string lastName, string email)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        user.Update(firstName, lastName, email);
        _userRepository.Update(user);

    }

    public User GetUserById(Guid id)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        return user;
    }
}