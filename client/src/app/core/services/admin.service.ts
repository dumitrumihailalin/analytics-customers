import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers() {
    return this.http.get<any[]>(`${this.api}/admin/users`);
  }

  createUser(req: { fullName: string; email: string; password: string }) {
    return this.http.post<any>(`${this.api}/admin/users`, req);
  }

  toggleActive(id: string) {
    return this.http.patch(`${this.api}/admin/users/${id}/toggle-active`, {});
  }

  updateSubscription(id: string, plan: string) {
    return this.http.patch(`${this.api}/admin/users/${id}/subscription`, JSON.stringify(plan), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
