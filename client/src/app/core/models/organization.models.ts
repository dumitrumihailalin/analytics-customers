export interface Organization {
  id: string;
  name: string;
  country: string | null;
  isActive: boolean;
  createdAt: string;
  address: string | null;
  webSite: string | null;
  userCount: number;
  storeCount: number;
}

export interface CreateOrganizationRequest {
  name: string;
  country: string | null;
  address: string | null;
  webSite: string | null;
}

export interface UpdateOrganizationRequest {
  name: string;
  country: string | null;
  address: string | null;
  webSite: string | null;
}
