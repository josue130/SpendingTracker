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
        public double Amount { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CategoryIncome CategoryIncome { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
