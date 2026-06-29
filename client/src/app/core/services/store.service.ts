import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Store, CreateStoreRequest, UpdateStoreRequest } from '../models/store.models';
import { ApiKey, GeneratedApiKey, CreateApiKeyRequest } from '../models/apikey.models';

@Injectable({ providedIn: 'root' })
export class StoreService {
  private storesApi = `${environment.apiUrl}/stores`;
  private apiKeysApi = `${environment.apiUrl}/apikeys`;

  constructor(private http: HttpClient) {}

  getAll(organizationId?: string) {
    const params = organizationId ? `?organizationId=${organizationId}` : '';
    return this.http.get<Store[]>(`${this.storesApi}${params}`);
  }

  create(req: CreateStoreRequest) {
    return this.http.post<Store>(this.storesApi, req);
  }

  update(id: string, req: UpdateStoreRequest) {
    return this.http.put<Store>(`${this.storesApi}/${id}`, req);
  }

  delete(id: string) {
    return this.http.delete(`${this.storesApi}/${id}`);
  }

  getApiKeys(storeId: string) {
    return this.http.get<ApiKey[]>(`${this.apiKeysApi}?storeId=${storeId}`);
  }

  generateApiKey(req: CreateApiKeyRequest) {
    return this.http.post<GeneratedApiKey>(this.apiKeysApi, req);
  }

  deactivateApiKey(id: string) {
    return this.http.patch<{ id: string; isActive: boolean }>(`${this.apiKeysApi}/${id}/deactivate`, {});
  }
}
