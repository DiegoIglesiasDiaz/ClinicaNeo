using Domain.Enums;
using Domain.Models;
namespace Application.Interfaces;

public interface IUserService
{
    User CreateUser(User user);
    void UpdateUser(User user);
    User GetUserById(Guid id);
}