import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'ac_token';
  private readonly USER_KEY = 'ac_user';

  private _user = signal<AuthResponse | null>(this.loadUser());

  readonly user = this._user.asReadonly();
  readonly isLoggedIn = computed(() => this._user() !== null);
  readonly isAdmin = computed(() => this._user()?.level === 'PlatformAdmin');
  readonly isOrgAdmin = computed(() => {
    const lvl = this._user()?.level;
    return lvl === 'PlatformAdmin' || lvl === 'OrgAdmin';
  });

  constructor(private http: HttpClient, private router: Router) {}

  login(req: LoginRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, req)
      .pipe(tap(res => this.persist(res)));
  }

  register(req: RegisterRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, req)
      .pipe(tap(res => this.persist(res)));
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this._user.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  private persist(res: AuthResponse) {
    localStorage.setItem(this.TOKEN_KEY, res.token);
    localStorage.setItem(this.USER_KEY, JSON.stringify(res));
    this._user.set(res);
  }

  private loadUser(): AuthResponse | null {
    const raw = localStorage.getItem(this.USER_KEY);
    return raw ? JSON.parse(raw) : null;
  }
}
