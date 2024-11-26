using SpendingTracker.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;

        public Users( Guid id, string fullName, string userName, string email, string password)
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            Password = password;
        }

        public static Users Create(string fullName, string userName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.", nameof(fullName));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("User name is required.", nameof(userName));
            if (string.IsNullOrWhiteSpace(email) || ValueObjects.Email.Create(email) is null)
                throw new ArgumentException("Invalid email format.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            Guid id = Guid.NewGuid();
            
            return new Users(id, fullName, userName, email, password);
        }

        private Users()
        {
            
        }
    }
}
