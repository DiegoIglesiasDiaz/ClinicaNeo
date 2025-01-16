using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Http.Json;
using Domain.Models;
namespace ClinicaNeo.Pages
{
    public partial class ScheduleComponent
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        private List<Schedule>? horasDisponibles;

        [Parameter]
        public Schedule? HoraSeleccionada { get; set; }

        private Schedule? internalHoraSeleccionada
        {
            get => HoraSeleccionada;
            set
            {
                if (HoraSeleccionada != value)
                {
                    HoraSeleccionada = value;
                    horaSeleccionadaChanged.InvokeAsync(value); // Notify the parent component
                }
            }
        }

        // Event callback for two-way binding
        [Parameter]
        public EventCallback<Schedule?> horaSeleccionadaChanged { get; set; }

        public async Task CargarCitas(DateTime date)
        {
            try
            {
                // Call the API endpoint with the date as a query parameter
                var response = await HttpClient.GetFromJsonAsync<List<Schedule>>($"api/schedule/GetActiveByDate?date={date.ToString("yyyy-MM-dd")}");
                var horasDisponiblesApi = new List<Schedule>();
                if (response != null)
                {                  
                    foreach (var sch in response)
                    {
                        horasDisponiblesApi.Add(sch);
                    }
                }
                horasDisponibles = horasDisponiblesApi; // Update the available times
                StateHasChanged(); // Notify the UI about the changes
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching schedules: {ex.Message}");
                horasDisponibles = new List<Schedule>(); // Handle error gracefully
            }
        }
        private void SeleccionarHora(Schedule hora)
        {
            internalHoraSeleccionada = hora;
        }
    }
}
