using Eventia.Domain.Enums;

namespace Eventia.Application.DTOs.Auth;

public record RegisterRequest(string Name, string Email, string Password, Role Role);
