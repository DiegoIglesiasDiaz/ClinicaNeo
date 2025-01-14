using Domain.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace ClinicaNeo.Pages
{
    public partial class Form
    {
        // The property to bind
        [Parameter]
        public Patient patient { get; set; }

        private Patient InternalPatient
        {
            get => patient;
            set
            {
                if (patient != value)
                {
                    patient = value;
                    PatientChanged.InvokeAsync(value); // Notify the parent component
                }
            }
        }

        // Event callback for two-way binding
        [Parameter]
        public EventCallback<Patient> PatientChanged { get; set; }

        // Reference to the MudForm
        public MudForm form;

        private string ValidateDniNie(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "El campo DNI/NIE es obligatorio.";

            var nifRegex = new Regex(@"^[0-9]{8}[TRWAGMYFPDXBNJZSQVHLCKE]$", RegexOptions.IgnoreCase);
            var nieRegex = new Regex(@"^[XYZ][0-9]{7}[TRWAGMYFPDXBNJZSQVHLCKE]$", RegexOptions.IgnoreCase);

            if (!nifRegex.IsMatch(input) && !nieRegex.IsMatch(input))
                return "El DNI o NIE introducido no es válido.";

            return null; // Si pasa las validaciones, no hay error.
        }


        private string ValidatePhone(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "El campo Teléfono es obligatorio.";

            var phoneRegex = new Regex(@"^[6789]\d{8}$", RegexOptions.Compiled);

            if (!phoneRegex.IsMatch(input))
                return "El número de teléfono debe de ser válido";

            return null; // Sin errores
        }

    }
}
