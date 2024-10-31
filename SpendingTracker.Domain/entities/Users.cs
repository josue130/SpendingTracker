using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string FullNmae { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
