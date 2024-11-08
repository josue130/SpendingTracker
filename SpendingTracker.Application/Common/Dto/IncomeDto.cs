using SpendingTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Dto
{
    public class IncomeDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
