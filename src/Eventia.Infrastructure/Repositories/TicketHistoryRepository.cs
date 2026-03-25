using Eventia.Domain.Entities;
using Eventia.Domain.Interfaces;
using Eventia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Eventia.Infrastructure.Repositories;

public class TicketHistoryRepository : ITicketHistoryRepository
{
    private readonly AppDbContext _context;

    public TicketHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TicketHistory history)
    {
        await _context.TicketHistories.AddAsync(history);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TicketHistory>> GetByTicketIdAsync(Guid ticketId)
    {
        return await _context.TicketHistories
            .Where(h => h.TicketId == ticketId)
            .Include(h => h.PerformedBy)
            .OrderByDescending(h => h.Timestamp)
            .ToListAsync();
    }
}
