using Eventia.Domain.Entities;

namespace Eventia.Domain.Interfaces;

public interface ITicketHistoryRepository
{
    Task AddAsync(TicketHistory history);
    Task<IEnumerable<TicketHistory>> GetByTicketIdAsync(Guid ticketId);
}
