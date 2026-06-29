namespace AnalyticsCustomers.Api.DTOs;

public record CreateStoreRequest(Guid OrganizationId, string StoreName, string? Street, string? Country, string? Website);
public record UpdateStoreRequest(string StoreName, string? Street, string? Country, string? Website);

public record StoreResponse(
    Guid Id,
    Guid OrganizationId,
    string OrganizationName,
    string StoreName,
    string? Street,
    string? Country,
    string? Website,
    DateTime CreatedAt,
    int ApiKeyCount
);
