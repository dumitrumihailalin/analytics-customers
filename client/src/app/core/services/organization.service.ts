import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Organization, CreateOrganizationRequest, UpdateOrganizationRequest } from '../models/organization.models';

@Injectable({ providedIn: 'root' })
export class OrganizationService {
  private api = `${environment.apiUrl}/organizations`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Organization[]>(this.api);
  }

  create(req: CreateOrganizationRequest) {
    return this.http.post<Organization>(this.api, req);
  }

  update(id: string, req: UpdateOrganizationRequest) {
    return this.http.put<Organization>(`${this.api}/${id}`, req);
  }

  toggleActive(id: string) {
    return this.http.patch<{ id: string; isActive: boolean }>(`${this.api}/${id}/toggle-active`, {});
  }

  delete(id: string) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
