using Eventia.Application.Common;
using Eventia.Domain.Enums;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Users.Queries;

public record GetAllUsersQuery() : IRequest<BaseResponse<IEnumerable<UserListResponse>>>;

public record UserListResponse(Guid Id, string Name, string Email, Role Role, bool IsActive, DateTime CreatedAt);

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<IEnumerable<UserListResponse>>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseResponse<IEnumerable<UserListResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();
        
        var response = users.Select(u => new UserListResponse(
            u.Id,
            u.Name,
            u.Email,
            u.Role,
            u.IsActive,
            u.CreatedAt
        ));

        return BaseResponse<IEnumerable<UserListResponse>>.SuccessResult(response);
    }
}
