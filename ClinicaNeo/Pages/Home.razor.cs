using Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http;
using System.Net.Http.Json;

namespace ClinicaNeo.Pages
{
    public partial class Home
    {
        [Inject]
        public HttpClient _httpClient { get; set; }
        [Inject]
        ISnackbar Snackbar { get; set; }
        MudStepper mudStepper;
        ScheduleComponent scheduleComponent;
        DateTime? selectedDate;
        Schedule? selectedHour;
        string appointmentNotes;
        Patient patient = new Patient();
        int _index;
        bool _completed;
        bool ConditionsCheckbox;
        private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
        {
            if (arg.Action == StepAction.Complete)
            {
                // Occurs when clicking next
                await ControlStepCompletion(arg, _index); 
            }
            else if (arg.Action == StepAction.Activate)
            {
                // Occurs when clicking a step header with the mouse
                if (arg.StepIndex - 1 == _index || arg.StepIndex < _index)
                {
                    await ControlStepCompletion(arg, _index);
                }
                else
                {
                    arg.Cancel = true;
                }
            }
        }

        private async Task ControlStepCompletion(StepperInteractionEventArgs arg, int index)
        {
            switch (index)
            {
                case 0:
                    if (selectedDate == null)
                    {
                        Snackbar.Add("No has Seleccionado Ninguna Fecha", Severity.Error);
                        arg.Cancel = true;
                    }
                    else
                    {
                        await scheduleComponent.CargarCitas((DateTime)selectedDate);
                    }
                    break;
                case 1:
                    if (selectedHour == null && (arg.StepIndex == index || arg.StepIndex - 1 == index)) //Moves one forward
                    {
                        Snackbar.Add("No has Seleccionado Ninguna Hora", Severity.Error);
                        arg.Cancel = true;
                    }
                    break;
                case 2:
                    if (!patient.isValid() && (arg.StepIndex == index || arg.Action == StepAction.Complete))
                    {
                        Snackbar.Add("No has Completado los Datos.", Severity.Error);
                        arg.Cancel = true;
                    }
                    break;
            }
        }
        private async void OnClickReservar(MudStepper stepper)
        {
            if(patient.isValid() && selectedDate != null && selectedHour != null)
            {
                if (ConditionsCheckbox)
                {
                    var newAppointment = new Appointment((DateTime)selectedDate, selectedHour, patient, Domain.Enums.AppointmentStatus.Scheduled,appointmentNotes);
                    var response = await _httpClient.PostAsJsonAsync<Appointment>("api/Appointment", newAppointment);
                    _completed = true;
                    await stepper.NextStepAsync();
                }
                else
                {
                    Snackbar.Add("Debes de aceptar los términos y condiciones de uso.", Severity.Error);
                }
                
            }
            else
            {
                Snackbar.Add("Completa Correctamente los Datos.", Severity.Error);
            }
           
        }
        private async Task OnClickSendEmail()
        {
            var email = new Email()
            {
                Body = "Esto es una prueba de email!!",
                IsHtml = false,
                Subject = "Hola Mundo",
                ToEmail="diegoiglesiasdiaz11@gmail.com"
            };
            try
            {
                var result = await _httpClient.PostAsJsonAsync<Email>("api/Email", email);
                if (result.IsSuccessStatusCode)
                    Snackbar.Add("Email Enviado Correctamente", Severity.Success);
                else
                    Snackbar.Add("Fallo al enviar el email", Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Fallo al enviar el email", Severity.Error);
            }
            
        }
    }
}
