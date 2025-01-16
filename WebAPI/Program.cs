using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Default connection string for development
var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("DefaultConnection") // Local development connection string
    : BuildConnectionStringFromSecrets(builder.Configuration); // Production connection string using GitHub secrets

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with the connection string
builder.Services.AddDbContext<ClinicaNeoContext>(options =>
    options.UseSqlServer(connectionString));

// Register Identity services
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ClinicaNeoContext>() // Use the DbContext for Identity
.AddDefaultTokenProviders(); // Optional: for password reset, etc.

// Register application services (repositories, services, etc.)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<INonWorkingDayRepository, NonWorkingDayRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
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
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Method to build the connection string from GitHub Secrets in production
string BuildConnectionStringFromSecrets(IConfiguration configuration)
{
    var dbServer = configuration["DB_SERVER"];
    var dbName = configuration["DB_NAME"];
    var dbUser = configuration["DB_USER"];
    var dbPassword = configuration["DB_PASSWORD"];

    return $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword}";
}
