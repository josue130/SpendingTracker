
namespace SpendingTracker.Application.Common.Dto
{
    public class ExpenseDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
