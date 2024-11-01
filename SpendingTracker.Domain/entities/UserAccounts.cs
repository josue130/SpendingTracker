using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendingTracker.Domain.entities
{
    public class UserAccounts
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Accounts Accounts { get; set; } = null!;
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;
    }
}
