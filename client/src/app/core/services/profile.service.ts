import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ProfileRequest, ProfileResponse } from '../models/profile.models';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  get() {
    return this.http.get<ProfileResponse>(`${this.api}/profile`);
  }

  update(req: ProfileRequest) {
    return this.http.put<ProfileResponse>(`${this.api}/profile`, req);
  }
}
