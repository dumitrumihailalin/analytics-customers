import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { SubscriptionInfo, SubscriptionKey } from '../models/subscription.models';

@Injectable({ providedIn: 'root' })
export class SubscriptionService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getInfo() {
    return this.http.get<SubscriptionInfo>(`${this.api}/subscription/info`);
  }

  getKeyHistory() {
    return this.http.get<SubscriptionKey[]>(`${this.api}/subscription/key/history`);
  }

  generateKey(storeId?: string) {
    return this.http.post<{ id: string; key: string; issuedAt: string; expiresAt: string; isActive: boolean; storeId: string | null; organizationId: string | null }>(
      `${this.api}/subscription/key/generate`, storeId ? { storeId } : {}
    );
  }

  assignStore(storeId: string) {
    return this.http.patch<{ id: string; key: string; storeId: string }>(
      `${this.api}/subscription/key/assign-store`, { storeId }
    );
  }
}
