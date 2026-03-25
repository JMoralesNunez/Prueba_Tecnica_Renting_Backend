using Eventia.Application.Common;
using Eventia.Application.DTOs.Tickets;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Domain.Interfaces;
using MediatR;


namespace Eventia.Application.Features.Tickets.Commands;

public record UpdateTicketCommand(Guid Id, UpdateTicketRequest Request) : IRequest<BaseResponse<TicketResponse>>;

public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, BaseResponse<TicketResponse>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketHistoryRepository _historyRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTicketHandler(ITicketRepository ticketRepository, ITicketHistoryRepository historyRepository, ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _historyRepository = historyRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<TicketResponse>> Handle(UpdateTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(command.Id);
        if (ticket == null)
        {
            return BaseResponse<TicketResponse>.Failure("Ticket not found");
        }

        var oldStatus = ticket.Status;
        ticket.Title = command.Request.Title;
        ticket.Description = command.Request.Description;
        ticket.Status = command.Request.Status;
        ticket.AssigneeId = command.Request.AssigneeId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _ticketRepository.UpdateAsync(ticket);

        // Add history record
        await _historyRepository.AddAsync(new Eventia.Domain.Entities.TicketHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            Action = "Ticket Updated",
            PerformedById = _currentUserService.UserId ?? Guid.Empty,
            Timestamp = DateTime.UtcNow,
            Notes = $"Updated details. Status: {oldStatus} -> {ticket.Status}"
        });


        var response = new TicketResponse(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Status,
            ticket.Assignee?.Name,
            ticket.CreatedBy?.Name ?? "Unknown",
            ticket.EventName,
            ticket.CreatedAt,
            ticket.UpdatedAt
        );

        return BaseResponse<TicketResponse>.SuccessResult(response, "Ticket updated successfully");
    }
}
