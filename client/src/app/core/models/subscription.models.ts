export interface SubscriptionKey {
  id: string;
  key: string;
  issuedAt: string;
  expiresAt: string;
  isActive: boolean;
  daysRemaining: number;
}

export interface SubscriptionInfo {
  plan: string;
  isActive: boolean;
  startDate: string;
  endDate: string | null;
  key: string | null;
  daysRemaining: number;
}
