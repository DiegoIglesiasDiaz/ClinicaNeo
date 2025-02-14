﻿using Domain.Models;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ClinicaNeoContext _context;

    public UserRepository(ClinicaNeoContext context)
    {
        _context = context;
    }

    public User Add(User user)
    {
       var entity = _context.Users.Add(user);
        _context.SaveChanges();
        return entity.Entity;
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public User? GetById(Guid id)
    {
        return _context.Users.SingleOrDefault(u => u.Id == id);
    }
}