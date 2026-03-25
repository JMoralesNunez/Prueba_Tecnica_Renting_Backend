using Eventia.Application.Common;
using Eventia.Application.DTOs.Tickets;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Tickets.Queries;

public record GetAllTicketsQuery() : IRequest<BaseResponse<IEnumerable<TicketResponse>>>;

public class GetAllTicketsHandler : IRequestHandler<GetAllTicketsQuery, BaseResponse<IEnumerable<TicketResponse>>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetAllTicketsHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<BaseResponse<IEnumerable<TicketResponse>>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetAllAsync();
        
        var response = tickets.Select(t => new TicketResponse(
            t.Id,
            t.Title,
            t.Description,
            t.Status,
            t.Assignee?.Name,
            t.CreatedBy?.Name ?? "Unknown",
            t.EventName,
            t.CreatedAt,
            t.UpdatedAt
        ));

        return BaseResponse<IEnumerable<TicketResponse>>.SuccessResult(response);
    }
}
