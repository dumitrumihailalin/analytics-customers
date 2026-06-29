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

  generateKey() {
    return this.http.post<{ id: string; key: string; issuedAt: string; expiresAt: string; isActive: boolean; organizationId: string | null }>(
      `${this.api}/subscription/key/generate`, {}
    );
  }
}
