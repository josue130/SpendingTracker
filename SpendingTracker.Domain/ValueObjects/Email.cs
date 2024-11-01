using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; init; }
        private Email(string value) => Value = value;
        public static Email? Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
              
                return null;

            }

            return new Email(value);
        }


    }
}
