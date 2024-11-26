using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class Expense
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Accounts Accounts { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryExpense CategoryExpense { get; set; } = null!;

        public Expense(Guid id, string description, double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            Id = id;
            Description = description;
            Amount = amount;
            Date = date;
            AccountId = accountId;
            CategoryId = categoryId;
        }

        public static Expense Create(string description, double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            Validate(amount, date, accountId, categoryId);
            return new Expense(Guid.NewGuid(), description, amount, date, accountId, categoryId);
        }


        public static Expense Update(Guid expenseId, string description, double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            if (expenseId == Guid.Empty) throw new ArgumentException("ExpenseId is required", nameof(expenseId));
            Validate(amount, date, accountId, categoryId);
            return new Expense (expenseId,description,amount,date,accountId,categoryId);
        }

        private static void Validate( double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new ArgumentException("categoryId is required", nameof(categoryId));
            if (accountId == Guid.Empty) throw new ArgumentException("accountId is required", nameof(accountId));
            if (date == default) throw new ArgumentException("Date can't be null or default.", nameof(date));
            if (amount < 0) throw new ArgumentException("amount must be greater than 0.", nameof(amount));
        }
        private Expense()
        {

        }
    }
}
