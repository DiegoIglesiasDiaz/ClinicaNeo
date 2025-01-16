using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly ClinicaNeoContext _context;

    public ScheduleRepository(ClinicaNeoContext context)
    {
        _context = context;
    }

    public Schedule Add(Schedule schedule)
    {
        var entity = _context.Schedules.Add(schedule);
        _context.SaveChanges();
        return entity.Entity;
    }

    public void Update(Schedule schedule)
    {
        _context.Schedules.Update(schedule);
        _context.SaveChanges();
    }
    public IList<Schedule> GetAll()
    {
        return _context.Schedules.ToList();
    }
    public async Task<List<Schedule>> GetAvailableSchedulesForSpecificDateAsync(DateTime date)
    {
        // Using the AppointmentStatus.Cancelled.ToString() for the cancelled status
        var appointmentStatusCancelled = AppointmentStatus.Cancelled.ToString();

        var availableSchedules = await _context.Schedules
            .FromSqlRaw(@"
            SELECT s.*, a.Date
            FROM [dbo].[Schedules] s
            LEFT JOIN Appointments a 
                ON a.Date = {0}
                AND a.Status <> {1}  -- Using the AppointmentStatus.Cancelled enum value
                AND (
                    (a.StartTime < s.EndTime AND a.EndTime > s.StartTime)
                    OR (s.StartTime < a.EndTime AND s.EndTime > a.StartTime)
                )
            WHERE s.IsActive = 1
                AND (a.Date IS NULL OR a.Status = {1})
            ORDER BY s.StartTime;
        ", date.ToString("yyyy-MM-dd"), appointmentStatusCancelled)  // Bind the enum value as a parameter
            .AsNoTracking()
            .ToListAsync();

        return availableSchedules;
    }

    public async Task<List<Schedule>> GetActiveSchedulesAsync()
    {

        return _context.Schedules.Where(x=> x.IsActive).ToList();
    }
}
