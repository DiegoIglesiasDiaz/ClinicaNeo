using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password);
        }
    }
}
