using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class NonWorkingDayRepository : INonWorkingDayRepository
{
    private readonly ClinicaNeoContext _context;

    public NonWorkingDayRepository(ClinicaNeoContext context)
    {
        _context = context;
    }
    public NonWorkingDay Add(NonWorkingDay nonWorkingDay)
    {
        var entity = _context.NonWorkingDays.Add(nonWorkingDay);
        _context.SaveChanges();
        return entity.Entity;
    }

    public void Update(NonWorkingDay nonWorkingDay)
    {
        _context.NonWorkingDays.Update(nonWorkingDay);
        _context.SaveChanges();
    }
    public IList<NonWorkingDay> GetAll()
    {
        return _context.NonWorkingDays.ToList();
    }
    public async Task<NonWorkingDay> GetByDate(DateTime date)
    {
        return await _context.NonWorkingDays.FirstAsync(x => x.Date == date);
    }
    public async Task<bool> IsNonWorkingDayAsync(DateTime date)
    {
        return await _context.NonWorkingDays.AnyAsync(n => n.Date == date);
    }


}
