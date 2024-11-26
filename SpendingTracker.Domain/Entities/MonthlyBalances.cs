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
            Validate(year, month, accountId);
            return new MonthlyBalances(Guid.NewGuid(), year, month, balance, accountId);
        }
        public static MonthlyBalances Update(Guid MonthlyBalanceId, int year, int month, double balance, Guid accountId)
        {
            if (MonthlyBalanceId == Guid.Empty) throw new ArgumentException("MonthlyBalanceId is required", nameof(MonthlyBalanceId));
            Validate(year, month, accountId);
            return new MonthlyBalances(MonthlyBalanceId,year,month,balance,accountId);
        }
        private static void Validate(int year, int month, Guid accountId)
        {
            if (accountId == Guid.Empty) throw new ArgumentException("accountId is required", nameof(accountId));
            if (year < 1900 || year > 2100) throw new ArgumentException("Invalid year", nameof(year));
            if (month < 1 || month > 12) throw new ArgumentException("Invalid month", nameof(month));
        }
        private MonthlyBalances()
        {
            
        }
    }
    
}
