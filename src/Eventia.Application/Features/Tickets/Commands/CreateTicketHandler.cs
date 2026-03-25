using Eventia.Application.Common;
using Eventia.Application.DTOs.Tickets;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Domain.Enums;
using Eventia.Domain.Interfaces;
using MediatR;


namespace Eventia.Application.Features.Tickets.Commands;

public record CreateTicketCommand(CreateTicketRequest Request) : IRequest<BaseResponse<TicketResponse>>;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, BaseResponse<TicketResponse>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketHistoryRepository _historyRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTicketHandler(ITicketRepository ticketRepository, ITicketHistoryRepository historyRepository, ICurrentUserService currentUserService)
    {
        _ticketRepository = ticketRepository;
        _historyRepository = historyRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BaseResponse<TicketResponse>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var userId = _currentUserService.UserId;
        
        if (userId == null)
        {
            return BaseResponse<TicketResponse>.Failure("Unauthorized to create ticket");
        }

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            EventName = request.EventName,
            Status = TicketStatus.Open,
            CreatedById = userId.Value,
            AssigneeId = request.AssigneeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _ticketRepository.AddAsync(ticket);

        // Add history record
        await _historyRepository.AddAsync(new TicketHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            Action = "Ticket Created",
            PerformedById = userId.Value,
            Timestamp = DateTime.UtcNow,
            Notes = $"Initial status: {ticket.Status}"
        });


        // Reload to get navigation properties if needed, or just return basic
        var response = new TicketResponse(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Status,
            null, // Assignee name will be null initially unless we fetch it
            "You", // Since we just created it
            ticket.EventName,
            ticket.CreatedAt,
            ticket.UpdatedAt
        );

        return BaseResponse<TicketResponse>.SuccessResult(response, "Ticket created successfully");
    }
}

