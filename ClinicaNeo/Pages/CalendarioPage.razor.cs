using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace ClinicaNeo.Pages
{
    public partial class CalendarioPage
    {
        [Inject]
        public HttpClient _httpClient { get; set; }
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public DateTime? SelectedDate { get; set; }
        private int displayMonths;
        private double windowWidth;
        private List<NonWorkingDay> nonWorkingDays = new List<NonWorkingDay>();
        private List<DateTime> bookedDates = new List<DateTime>();
        private DateTime? internalSelectedDate
        {
            get => SelectedDate;
            set
            {
                if (SelectedDate != value)
                {
                    SelectedDate = value;
                    SelectedDateChanged.InvokeAsync(value); // Notify the parent component
                }
            }
        }
        [Parameter]
        public EventCallback<DateTime?> SelectedDateChanged { get; set; }
        private void UpdateDisplayMonths()
        {
            // Cambiar el valor basado en el tamaño de la pantalla
            if (windowWidth >= 640)  // Pantalla de escritorio
            {
                displayMonths = 2;
            }
            else
            {
                displayMonths = 1; // Móvil o tablet
            }
        }
        protected override async Task OnInitializedAsync()
        {
            // Determinar el número de meses a mostrar según el tamaño de la pantalla
            // Preload non-working days
            try
            {
                nonWorkingDays = await _httpClient.GetFromJsonAsync<List<NonWorkingDay>>("api/NonWorkingDay");
                bookedDates = await _httpClient.GetFromJsonAsync<List<DateTime>>("api/Appointment/BookedDates");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching non-working days: {ex.Message}");
            }
            UpdateDisplayMonths();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Obtener el ancho de la ventana desde JavaScript
                windowWidth = await JSRuntime.InvokeAsync<double>("eval", "window.innerWidth");
                UpdateDisplayMonths();
                StateHasChanged();
            }
        }
        private bool IsDateDisabledFunc(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;
            if (nonWorkingDays.Any(nwd => nwd.Date.Date == date.Date))
                return true;
            if (bookedDates.Any(bk => bk.Date.Date == date.Date))
                return true;
            return false;
        }
    }
}
