export interface Store {
  id: string;
  organizationId: string;
  organizationName: string;
  storeName: string;
  street: string | null;
  country: string | null;
  website: string | null;
  createdAt: string;
  apiKeyCount: number;
}

export interface CreateStoreRequest {
  organizationId: string;
  storeName: string;
  street: string | null;
  country: string | null;
  website: string | null;
}

export interface UpdateStoreRequest {
  storeName: string;
  street: string | null;
  country: string | null;
  website: string | null;
}
