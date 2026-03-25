namespace Eventia.Domain.Entities;

public class TicketHistory
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public string Action { get; set; } = string.Empty;
    public Guid PerformedById { get; set; }
    public User? PerformedBy { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}
