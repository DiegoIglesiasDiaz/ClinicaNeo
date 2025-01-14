using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string? Address { get; set; }
        [PersonalData]
        public string? PostCode { get; set; }
        [PersonalData]
        public DateTime DateOfBirth { get; set; }
        public Role Role { get; set; }
        public IList<Appointment> Appointments { get; set; }

        public User() 
        {
            Appointments = new List<Appointment>();
        }
        public User(string firstName, string lastName, string email, string phoneNumber, string address, string? postcode, DateTime dateOfBirth, Role role)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            DateOfBirth = dateOfBirth;
            Role = role;
            Appointments = new List<Appointment>();
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentException("First name cannot be empty.");

            if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentException("Last name cannot be empty.");

            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email))
                throw new ArgumentException("Email is invalid.");

            if (DateOfBirth < DateTime.Now)
                throw new ArgumentException("Date of birth must be in the past.");

            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentException("Address cannot be empty.");

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new ArgumentException("Phone number cannot be empty.");

            if (!Enum.IsDefined(typeof(Role), Role))
                throw new ArgumentException("Invalid role assigned.");
        }

        private bool IsValidEmail(string email)
        {
            // Simple regular expression for validating an email address.
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
    }
}
