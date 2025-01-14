﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClinicaNeo.Pages
{
    public partial class Calendar
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public DateTime? SelectedDate { get; set; }
        private int displayMonths;
        private double windowWidth;
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
        protected override void OnInitialized()
        {
            // Determinar el número de meses a mostrar según el tamaño de la pantalla
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
    }
}
