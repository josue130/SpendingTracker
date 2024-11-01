using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.entities
{
    public class Accounts
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;

        public Accounts(Guid id, string accountName, double amount, string description)
        {
            Id = id;
            AccountName = accountName;
            Amount = amount;
            Description = description;
        }
        public static Accounts Create(string accountName, double amount , string description)
        {
            Guid id = Guid.NewGuid();

            return new Accounts(id, accountName, amount, description);
        }
        private Accounts()
        {
            
        }
    }
}
