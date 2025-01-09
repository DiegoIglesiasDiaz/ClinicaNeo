﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }

        public Patient()
        {
            Id = Guid.NewGuid();
        }

        public bool isValid()
        {
         if(String.IsNullOrEmpty(Name)) return false;
         if(String.IsNullOrEmpty(Surnames)) return false;
         if(String.IsNullOrEmpty(Email) || !IsValidEmail(Email)) return false;
         return true;
        }
        private bool IsValidEmail(string email)
        {
            // Simple regular expression for validating an email address.
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]");
            return emailRegex.IsMatch(email);
        }
    }
    
}
