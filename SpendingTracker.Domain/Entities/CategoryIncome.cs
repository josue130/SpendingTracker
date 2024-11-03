using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class CategoryIncome
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
    }
}
