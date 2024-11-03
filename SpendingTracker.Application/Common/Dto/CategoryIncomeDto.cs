using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Application.Common.Dto
{
    public class CategoryIncomeDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public Guid UserId { get; set; }
    }
}
