using Domain.Enums;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicaNeoContext _context;

    public AppointmentRepository(ClinicaNeoContext context)
    {
        _context = context;
    }

    public Appointment Add(Appointment Appointment)
    {
        var entity = _context.Appointments.Add(Appointment);
        _context.SaveChanges();
        return entity.Entity;
    }

    public void Update(Appointment Appointment)
    {
        _context.Appointments.Update(Appointment);
        _context.SaveChanges();
    }

    public Appointment? GetById(int id)
    {
        return _context.Appointments.SingleOrDefault(u => u.Id == id);
    }

    public async Task<bool> IsAppointmentAvailableAsync(Appointment appointment)
    {
        return  !(await _context.Appointments.AnyAsync(a =>
                   a.Date == appointment.Date &&
                   ((a.StartTime < appointment.EndTime && a.EndTime > appointment.StartTime))));
    }
    public async Task<List<DateTime>> GetBookedDatesAsync()
    {
        var result = new List<DateTime>();
        var sqlQuery = @"
        SELECT a.Date
        FROM [dbo].[Schedules] s
        LEFT JOIN Appointments a 
            ON a.Status <> '"+AppointmentStatus.Cancelled.ToString()+@"'
            AND (
                (a.StartTime < s.EndTime AND a.EndTime > s.StartTime)
                OR (s.StartTime < a.EndTime AND s.EndTime > a.StartTime)
            )
        WHERE s.IsActive = 1
        GROUP BY a.Date
        HAVING COUNT(s.Id) = (SELECT COUNT(*) FROM [dbo].[Schedules] WHERE Date = a.Date AND IsActive = 1);";

        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = sqlQuery;
            command.CommandType = System.Data.CommandType.Text;

            if (command.Connection.State != System.Data.ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var date = reader.GetDateTime(reader.GetOrdinal("Date"));
                    result.Add(date);
                }
            }
        }

        // Optionally, sort the results
        return result.OrderBy(d => d).ToList();
    }






}