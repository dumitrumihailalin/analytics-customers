namespace AnalyticsCustomers.Api.DTOs;

public record CreateOrganizationRequest(string Name, string? Country, string? Address, string? WebSite);
public record UpdateOrganizationRequest(string Name, string? Country, string? Address, string? WebSite);

public record OrganizationResponse(
    Guid Id,
    string Name,
    string? Country,
    bool IsActive,
    DateTime CreatedAt,
    string? Address,
    string? WebSite,
    int UserCount,
    int StoreCount
);
