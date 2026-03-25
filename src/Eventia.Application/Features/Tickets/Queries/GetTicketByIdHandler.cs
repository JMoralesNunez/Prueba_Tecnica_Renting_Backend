using Eventia.Application.Common;
using Eventia.Application.DTOs.Tickets;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Tickets.Queries;

public record GetTicketByIdQuery(Guid Id) : IRequest<BaseResponse<TicketResponse>>;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, BaseResponse<TicketResponse>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketByIdHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<BaseResponse<TicketResponse>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var t = await _ticketRepository.GetByIdAsync(request.Id);
        if (t == null)
        {
            return BaseResponse<TicketResponse>.Failure("Ticket not found");
        }

        var response = new TicketResponse(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Assignee?.Name,
            t.CreatedBy?.Name ?? "Unknown",
            t.EventName,
            t.CreatedAt,
            t.UpdatedAt
        );

        return BaseResponse<TicketResponse>.SuccessResult(response);
    }
}
