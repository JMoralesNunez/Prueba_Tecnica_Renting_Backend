using Eventia.Application.Common;
using Eventia.Application.DTOs.Tickets;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Domain.Enums;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Tickets.Commands;

public record ChangeTicketStatusCommand(Guid Id, TicketStatus Status) : IRequest<BaseResponse<bool>>;

public class ChangeTicketStatusHandler : IRequestHandler<ChangeTicketStatusCommand, BaseResponse<bool>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketHistoryRepository _historyRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangeTicketStatusHandler(ITicketRepository ticketRepository, ITicketHistoryRepository historyRepository, ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _historyRepository = historyRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.Id);
        if (ticket == null)
        {
            return BaseResponse<bool>.Failure("Ticket not found");
        }

        var oldStatus = ticket.Status;
        ticket.Status = request.Status;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);

        // Add history record
        await _historyRepository.AddAsync(new TicketHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            Action = "Status Changed",
            PerformedById = _currentUserService.UserId ?? Guid.Empty,
            Timestamp = DateTime.UtcNow,
            Notes = $"Changed from {oldStatus} to {ticket.Status}"
        });

        return BaseResponse<bool>.SuccessResult(true, $"Ticket status changed correctly to {request.Status}");
    }
}

