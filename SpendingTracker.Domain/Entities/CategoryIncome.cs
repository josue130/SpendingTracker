using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.Entities
{
    public class CategoryIncome
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
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
            Validate(categoryName,color,icon,userId);
            return new CategoryIncome(Guid.NewGuid(), categoryName, color, icon, userId);
        }

        public static CategoryIncome Update(Guid categoryIncomeId,string categoryName, string color, string icon, Guid userId)
        {
            if (categoryIncomeId == Guid.Empty) throw new ArgumentException("CategoryIncomeId is required", nameof(categoryIncomeId));
            Validate(categoryName, color, icon, userId);
            return new CategoryIncome(categoryIncomeId,categoryName,color,icon,userId);
        }

        private static void Validate(string categoryName, string color, string icon, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(color)) throw new ArgumentException("Color is required.", nameof(color));
            if (string.IsNullOrWhiteSpace(categoryName)) throw new ArgumentException("Category name is required.", nameof(categoryName));
            if (string.IsNullOrWhiteSpace(icon)) throw new ArgumentException("Icon is required.", nameof(icon));
            if (userId == Guid.Empty) throw new ArgumentException("UserId is required.", nameof(userId));
        }

        private CategoryIncome()
        {
            
        }
    }
}
