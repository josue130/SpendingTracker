using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
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
            Validate(accountName, amount, description);

            Guid id = Guid.NewGuid();

            return new Accounts(id, accountName, amount, description);
        }

        public static Accounts Update(Guid accountId,string accountName, double amount, string description)
        {
            if (accountId == Guid.Empty) throw new ArgumentException("AccountId is required", nameof(accountId));

            Validate(accountName, amount, description);

            return new Accounts(accountId, accountName, amount, description);
        }

        private static void Validate(string accountName, double amount, string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description is required", nameof(description));
            if (string.IsNullOrWhiteSpace(accountName)) throw new ArgumentException("accountName is required", nameof(accountName));
            if (amount < 0) throw new ArgumentException("amount must be greater than 0.", nameof(amount));
        }

        private Accounts()
        {
            
        }
    }
}
