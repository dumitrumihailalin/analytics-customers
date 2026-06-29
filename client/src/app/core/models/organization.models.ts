export interface Organization {
  id: string;
  name: string;
  country: string | null;
  isActive: boolean;
  createdAt: string;
  userCount: number;
  storeCount: number;
}

export interface CreateOrganizationRequest {
  name: string;
  country: string | null;
}

export interface UpdateOrganizationRequest {
  name: string;
  country: string | null;
}
