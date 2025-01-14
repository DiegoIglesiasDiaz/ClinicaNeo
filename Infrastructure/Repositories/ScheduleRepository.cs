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
    public async Task<List<Schedule>> GetActiveSchedulesByDateAsync(DateTime date)
    {

        return _context.Schedules
         .FromSqlInterpolated($@"
            SELECT s.*, a.Date
            FROM [dbo].[Schedules] s
            left join Appointments a on a.StartTime = s.StartTime and a.EndTime = s.EndTime and a.Date = {date.ToString("yyyyMMdd")}
            where IsActive = 1 and a.Date is null")
         .AsNoTracking()
         .ToList();
    }
    public async Task<List<Schedule>> GetActiveSchedulesAsync()
    {

        return _context.Schedules.Where(x=> x.IsActive).ToList();
    }
}
