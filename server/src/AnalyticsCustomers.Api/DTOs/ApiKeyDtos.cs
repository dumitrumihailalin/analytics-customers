namespace AnalyticsCustomers.Api.DTOs;

public record CreateApiKeyRequest(Guid StoreId, DateTime? ExpiresAt);

public record ApiKeyResponse(
    Guid Id,
    Guid StoreId,
    string StoreName,
    string Prefix,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    DateTime? LastUsedAt
);

public record GeneratedApiKeyResponse(
    Guid Id,
    Guid StoreId,
    string StoreName,
    string Prefix,
    string RawKey,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? ExpiresAt
);
