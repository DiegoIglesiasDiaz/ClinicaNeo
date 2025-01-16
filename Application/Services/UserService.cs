using Application.Interfaces;
using Infrastructure.Interfaces;
using Domain.Models;
using Domain.Enums;
using Infrastructure.Repositories;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User CreateUser(User user)
    {
        _userRepository.Add(user);
        return user;
    }

    public void UpdateUser(User user)
    {
        if (_userRepository.GetById(user.Id) == null)
        {
            throw new KeyNotFoundException("User not found");
        }
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