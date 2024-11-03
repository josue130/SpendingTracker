using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;

        public CategoryIncome(Guid id, string categoryName, string color, string icon, Guid userId)
        {
            Id = id;
            CategoryName = categoryName;
            Color = color;
            Icon = icon;
            UserId = userId;
        }

        public static CategoryIncome Create(string categoryName, string color, string icon, Guid userId) 
        {
            return new CategoryIncome {
                Id = Guid.NewGuid(),
                CategoryName = categoryName,
                Color = color,
                Icon = icon
                UserId = userId
            };
        }

        private CategoryIncome()
        {
            
        }
    }
}
