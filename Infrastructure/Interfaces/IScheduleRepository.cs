namespace Infrastructure.Interfaces;
using Domain.Models;
public interface IScheduleRepository
{
    Schedule Add(Schedule schedule);
    void Update(Schedule schedule);
    IList<Schedule> GetAll();
    Task<List<Schedule>> GetActiveSchedulesByDateAsync(DateTime date);
    Task<List<Schedule>> GetActiveSchedulesAsync();
}
