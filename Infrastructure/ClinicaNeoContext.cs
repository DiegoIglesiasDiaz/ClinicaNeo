using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure
{
    public class ClinicaNeoContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ClinicaNeoContext(DbContextOptions<ClinicaNeoContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Enum to be saved as a string
            var roleConverter = new ValueConverter<Role, string>(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v)
            );

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(roleConverter);

        }
    }
}
