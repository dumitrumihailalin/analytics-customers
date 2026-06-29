import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AdminStats, DashboardSummary, OrderRequest } from '../models/analytics.models';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getDashboard(year?: number) {
    if (year) {
      return this.http.get<DashboardSummary>(`${this.api}/analytics/dashboard`, {
        params: { year: year.toString() }
      });
    }
    return this.http.get<DashboardSummary>(`${this.api}/analytics/dashboard`);
  }

  getAdminStats() {
    return this.http.get<AdminStats>(`${this.api}/analytics/admin/stats`);
  }

  submitOrders(orders: OrderRequest[]) {
    return this.http.post(`${this.api}/orders/bulk`, { orders });
  }
}
