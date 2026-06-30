namespace AnalyticsCustomers.Api.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string? FullName = null
);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, string Email, string? FullName, string Level);

public record ProfileRequest(string? FullName);

public record ProfileResponse(
    Guid Id,
    string Email,
    string? FullName,
    string Level,
    DateTime CreatedAt
);

public record CreateAdminUserRequest(string FullName, string Email, string Password);
