using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class MonthlyBalances
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Balance { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Accounts Accounts { get; set; } = null!;
        public MonthlyBalances(Guid id, int year, int month, double balance, Guid accountId)
        {
            Id = id;
            Year = year;
            Month = month;
            Balance = balance;
            AccountId = accountId;
            
        }
        public static MonthlyBalances Create(int year, int month, double balance, Guid accountId)
        {
            return new MonthlyBalances
            {
                Id = Guid.NewGuid(),
                Year = year,
                Month = month,
                Balance = balance,
                AccountId = accountId
            };
        }
        private MonthlyBalances()
        {
            
        }
    }
    
}
