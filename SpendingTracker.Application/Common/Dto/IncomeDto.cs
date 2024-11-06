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

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "El Id de la cuenta es obligatorio.")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "El Id de la categoría es obligatorio.")]
        public Guid CategoryId { get; set; }
    }
}
