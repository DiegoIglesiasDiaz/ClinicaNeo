namespace Infrastructure.Interfaces;
using Domain.Models;
public interface INonWorkingDayRepository
{
    NonWorkingDay Add(NonWorkingDay nonWorkingDay);
    void Update(NonWorkingDay nonWorkingDay);
    IList<NonWorkingDay> GetAll();
    Task<NonWorkingDay> GetByDate(DateTime date);
    Task<bool> IsNonWorkingDayAsync(DateTime date);
}
