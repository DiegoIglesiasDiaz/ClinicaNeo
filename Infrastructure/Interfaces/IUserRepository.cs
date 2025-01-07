namespace Infrastructure.Interfaces;
using Domain.Models;
public interface IUserRepository
{
    void Add(User user);
    void Update(User user);
    User? GetById(Guid id);
}
