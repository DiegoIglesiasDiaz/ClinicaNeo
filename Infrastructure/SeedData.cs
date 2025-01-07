using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider, UserManager<User> userManager)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Check if there are any roles already in the database
        var rolesExist = await roleManager.Roles.AnyAsync();
        if (!rolesExist)
        {
            // Seed roles if they don't exist
            string[] roleNames = Enum.GetNames(typeof(Role)); // Admin, Doctor, Patient
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }

        // Check if there are any users already in the database
        var usersExist = await userManager.Users.AnyAsync();
        if (!usersExist)
        {
            // Seed default users
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = Role.Admin,
                    Address = " C/ Avenida España",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                new User
                {
                    UserName = "doctor@example.com",
                    Email = "doctor@example.com",
                    FirstName = "John",
                    LastName = "Doctor",
                    Address = " C/ Avenida España",
                    Role = Role.Doctor,
                    PhoneNumber = "123-456-7891",
                    DateOfBirth = new DateTime(1985, 5, 15)
                },
                new User
                {
                    UserName = "patient@example.com",
                    Email = "patient@example.com",
                    FirstName = "Jane",
                    LastName = "Patient",
                    Address = " C/ Avenida España",
                    Role = Role.Patient,
                    PhoneNumber = "123-456-7892",
                    DateOfBirth = new DateTime(1990, 3, 20)
                }
            };

            foreach (var user in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var createUserResult = await userManager.CreateAsync(user, "Abrete01!"); // Seed password
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, user.Role.ToString());
                    }
                }
            }
        }
    }
}
