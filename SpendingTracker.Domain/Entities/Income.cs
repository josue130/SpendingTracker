using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class Income
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Accounts Accounts { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryIncome CategoryIncome { get; set; } = null!;


        public Income(Guid id, string description, double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            Id = id;
            Description = description;
            Amount = amount;
            Date = date;
            AccountId = accountId;
            CategoryId = categoryId;
        }

        public static Income Create(string description, double amount, DateTime date, Guid accountId, Guid categoryId)
        {
            return new Income
            {
                Id = Guid.NewGuid(),
                Description = description,
                Amount = amount,
                Date = date,
                AccountId = accountId,
                CategoryId = categoryId
            };
        }
        private Income()
        {
            
        }
    }
}
