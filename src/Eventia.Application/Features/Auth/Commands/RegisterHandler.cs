using Eventia.Application.Common;
using Eventia.Application.DTOs.Auth;
using Eventia.Application.Interfaces;
using Eventia.Domain.Entities;
using Eventia.Domain.Interfaces;
using MediatR;

namespace Eventia.Application.Features.Auth.Commands;

public record RegisterCommand(RegisterRequest Request) : IRequest<BaseResponse<AuthResponse>>;

public class RegisterHandler : IRequestHandler<RegisterCommand, BaseResponse<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<BaseResponse<AuthResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return BaseResponse<AuthResponse>.Failure("Email already registered");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = request.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var token = _tokenService.GenerateToken(user);
        var response = new AuthResponse(token, user.Email, user.Name, user.Role.ToString());

        return BaseResponse<AuthResponse>.SuccessResult(response, "User registered successfully");
    }
}
