export interface ApiKey {
  id: string;
  storeId: string;
  storeName: string;
  prefix: string;
  isActive: boolean;
  createdAt: string;
  expiresAt: string | null;
  lastUsedAt: string | null;
}

export interface GeneratedApiKey extends ApiKey {
  rawKey: string;
}

export interface CreateApiKeyRequest {
  storeId: string;
  expiresAt: string | null;
}
