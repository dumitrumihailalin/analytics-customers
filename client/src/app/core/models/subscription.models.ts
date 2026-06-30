export interface SubscriptionKey {
  id: string;
  key: string;
  issuedAt: string;
  expiresAt: string;
  isActive: boolean;
  daysRemaining: number;
  storeId: string | null;
}

export interface SubscriptionInfo {
  plan: string;
  isActive: boolean;
  startDate: string;
  endDate: string | null;
  key: string | null;
  storeId: string | null;
  daysRemaining: number;
}
