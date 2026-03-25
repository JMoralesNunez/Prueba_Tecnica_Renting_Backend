using Eventia.Domain.Enums;

namespace Eventia.Application.DTOs.Tickets;

public record TicketResponse(
    Guid Id,
    string Title,
    string Description,
    TicketStatus Status,
    string? AssigneeName,
    string CreatedByName,
    string EventName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateTicketRequest(
    string Title,
    string Description,
    string EventName,
    Guid? AssigneeId
);

public record UpdateTicketRequest(
    string Title,
    string Description,
    TicketStatus Status,
    Guid? AssigneeId
);
