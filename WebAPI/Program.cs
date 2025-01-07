using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext
builder.Services.AddDbContext<ClinicaNeoContext>(options =>
    options.UseSqlServer(connectionString));

// Register Identity services
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    // Example: Ensure unique emails for users
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ClinicaNeoContext>() // Use the DbContext for Identity
.AddDefaultTokenProviders(); // Optional: for password reset, etc.

// Register application services (repositories, services, etc.)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed roles and users (make sure to configure the SeedData class)
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        await SeedData.SeedRolesAndUsersAsync(serviceProvider, userManager);
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
