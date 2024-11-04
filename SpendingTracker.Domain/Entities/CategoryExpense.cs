using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class CategoryExpense
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;

        public CategoryExpense(Guid id, string categoryName, string color, string icon, Guid userId)
        {
            Id = id;
            CategoryName = categoryName;
            Color = color;
            Icon = icon;
            UserId = userId;
        }

        public static CategoryExpense Create(string categoryName, string color, string icon, Guid userId)
        {
            return new CategoryExpense
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryName,
                Color = color,
                Icon = icon,
                UserId = userId
            };
        }

        private CategoryExpense()
        {

        }
    }
}
