using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Radzen;
using System.Net.Http.Json;
using Domain.Enums;
using System;
using MudBlazor;

namespace ClinicaNeo.Pages
{
    public partial class CalendarioPage
    {
        [Inject]
        public HttpClient _httpClient { get; set; }
        [Inject]
        ISnackbar Snackbar { get; set; }
        public IList<Appointment> Appointments { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Appointments = await _httpClient.GetFromJsonAsync<List<Appointment>>("api/Appointment");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching appointments: {ex.Message}");
            }
        }
        async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<Appointment> args)
        {
            Appointment? data = await DialogService.OpenAsync<EditAppointmentPage>("Cita", new Dictionary<string, object> { { "Appointment", args.Data } });
            if (data != null && data.Status == AppointmentStatus.Cancelled)
            {
                await _httpClient.PutAsJsonAsync<Appointment>("api/Appointment",data);
                Snackbar.Add("Cita Cancelada Correctamente.", Severity.Success);
                Appointments.Remove(args.Data);
            }

            await scheduler.Reload();
        }

    }

}
