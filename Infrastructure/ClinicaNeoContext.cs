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
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<NonWorkingDay> NonWorkingDays { get; set; }
        public DbSet<Schedule> Schedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var roleConverter = new ValueConverter<Role, string>(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v)
            );

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(roleConverter);

            var NonWorkingDayReasonConverter = new ValueConverter<NonWorkingDayReason, string>(
                v => v.ToString(),
                v => (NonWorkingDayReason)Enum.Parse(typeof(NonWorkingDayReason), v)
            );

            modelBuilder.Entity<NonWorkingDay>()
                .Property(u => u.Reason)
                .HasConversion(NonWorkingDayReasonConverter);

            var AppointmentStatusConverter = new ValueConverter<AppointmentStatus, string>(
                v => v.ToString(),
                v => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), v)
            );

            modelBuilder.Entity<Appointment>()
                .Property(u => u.Status)
                .HasConversion(AppointmentStatusConverter);

            modelBuilder.Entity<Appointment>()
                .Navigation(a => a.Patient)
                .AutoInclude();

        }
    }
}
