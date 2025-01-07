using Domain.Models;
namespace Application.Interfaces;

public interface IUserService
{
    User CreateUser(string firstName, string lastName, string email);
    void UpdateUser(Guid id, string firstName, string lastName, string email);
    User GetUserById(Guid id);
}