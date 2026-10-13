namespace FastFoodDeals.Core.Entities;

public class DealClick
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DealId { get; set; }
    public DateTime ClickedAtUtc { get; set; } = DateTime.UtcNow;
}
