namespace FastFoodDeals.Core.Entities;

public class FeedbackEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? DealId { get; set; }
    public bool IsPositive { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
