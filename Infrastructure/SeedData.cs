using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

public static class SeedData
{
    public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider, UserManager<User> userManager)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var dbContext = serviceProvider.GetRequiredService<ClinicaNeoContext>();

        // Seed roles
        var rolesExist = await roleManager.Roles.AnyAsync();
        if (!rolesExist)
        {
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

        // Seed users
        var usersExist = await userManager.Users.AnyAsync();
        if (!usersExist)
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = Role.Admin,
                    Address = "C/ Avenida España",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
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

        // Seed schedules
        var schedulesExist = await dbContext.Schedules.AnyAsync();
        if (!schedulesExist)
        {
            var schedules = new List<Schedule>
            {
                new Schedule { StartTime = TimeSpan.FromHours(9.5), EndTime = TimeSpan.FromHours(11.5), IsActive = true },
                new Schedule { StartTime = TimeSpan.FromHours(11.5), EndTime = TimeSpan.FromHours(13.5), IsActive = true },
                new Schedule { StartTime = TimeSpan.FromHours(15.5), EndTime = TimeSpan.FromHours(17.5), IsActive = true },
                new Schedule { StartTime = TimeSpan.FromHours(17.5), EndTime = TimeSpan.FromHours(19.5), IsActive = true }
            };

            dbContext.Schedules.AddRange(schedules);
            await dbContext.SaveChangesAsync();
        }

        // Seed non-working days
        var nonWorkingDaysExist = await dbContext.NonWorkingDays.AnyAsync();
        if (!nonWorkingDaysExist)
        {
            var nonWorkingDays = new List<NonWorkingDay>
            {
                new NonWorkingDay { Date = new DateTime(2025, 1, 1), Reason = NonWorkingDayReason.BankHoliday },
                new NonWorkingDay { Date = new DateTime(2025, 12, 25), Reason = NonWorkingDayReason.BankHoliday },
                new NonWorkingDay { Date = new DateTime(2025, 5, 1), Reason = NonWorkingDayReason.Vacations }
            };

            dbContext.NonWorkingDays.AddRange(nonWorkingDays);
            await dbContext.SaveChangesAsync();
        }

        // Seed patients
        var patientsExist = await dbContext.Patients.AnyAsync();
        if (!patientsExist)
        {
            var patients = new List<Patient>
            {
                new Patient { Name = "Jane", Surnames = "Doe", Email = "jane.doe@example.com" },
                new Patient { Name = "John", Surnames = "Smith", Email = "john.smith@example.com" }
            };

            dbContext.Patients.AddRange(patients);
            await dbContext.SaveChangesAsync();
        }

        // Seed appointments
        var appointmentsExist = await dbContext.Appointments.AnyAsync();
        if (!appointmentsExist)
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    PatientId = 1, // Assume the ID of Jane
                    Date = new DateTime(2025, 1, 15),
                    StartTime = TimeSpan.FromHours(9.5),
                    EndTime = TimeSpan.FromHours(11.5),
                    Status = AppointmentStatus.Scheduled,
                    Notes = "First visit"
                },
                new Appointment
                {
                    PatientId = 2, // Assume the ID of John
                    Date = new DateTime(2025, 1, 16),
                    StartTime = TimeSpan.FromHours(15.5),
                    EndTime = TimeSpan.FromHours(17.5),
                    Status = AppointmentStatus.Scheduled,
                    Notes = "Routine check-up"
                }
            };

            dbContext.Appointments.AddRange(appointments);
            await dbContext.SaveChangesAsync();
        }
    }
}
