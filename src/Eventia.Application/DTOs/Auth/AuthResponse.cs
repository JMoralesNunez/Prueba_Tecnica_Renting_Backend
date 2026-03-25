namespace Eventia.Application.DTOs.Auth;

public record AuthResponse(string Token, string Email, string Name, string Role);
