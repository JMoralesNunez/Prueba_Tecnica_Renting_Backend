using Eventia.Application.Common;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Tickets.Queries;

public record GetTicketHistoryQuery(Guid TicketId) : IRequest<BaseResponse<IEnumerable<TicketHistoryResponse>>>;

public record TicketHistoryResponse(Guid Id, string Action, string PerformerName, DateTime Timestamp, string? Notes);

public class GetTicketHistoryHandler : IRequestHandler<GetTicketHistoryQuery, BaseResponse<IEnumerable<TicketHistoryResponse>>>
{
    private readonly ITicketHistoryRepository _historyRepository;

    public GetTicketHistoryHandler(ITicketHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task<BaseResponse<IEnumerable<TicketHistoryResponse>>> Handle(GetTicketHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await _historyRepository.GetByTicketIdAsync(request.TicketId);
        
        var response = history.Select(h => new TicketHistoryResponse(
            h.Id,
            h.Action,
            h.PerformedBy?.Name ?? "Unknown",
            h.Timestamp,
            h.Notes
        ));

        return BaseResponse<IEnumerable<TicketHistoryResponse>>.SuccessResult(response);
    }
}
