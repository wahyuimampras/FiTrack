import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, switchMap, shareReplay, finalize } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface AuthUser {
  id: string;
  username: string;
  email: string;
  stravaConnected: boolean;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresIn: number;
  user: AuthUser;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly API = environment.apiUrl;

  // State dengan signal
  private _currentUser = signal<AuthUser | null>(null);
  private _accessToken = signal<string | null>(null);
  private _tokenExpiry = signal<Date | null>(null);
  private _refreshTimer: ReturnType<typeof setTimeout> | null = null;

  // Public readonly signals
  readonly currentUser = this._currentUser.asReadonly();
  readonly isLoggedIn = computed(() =>
    !!this._currentUser() &&
    !!this._tokenExpiry() &&
    new Date() < this._tokenExpiry()!
  );

  get token(): string | null {
    return this._accessToken();
  }

  constructor() {
    // Coba restore session dari localStorage saat app load
    this.restoreSession();
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.API}/auth/login`,
      credentials,
      { withCredentials: true }
    ).pipe(
      tap(res => this.handleAuthSuccess(res)),
      catchError(err => {
        this.clearAuth();
        return throwError(() => err);
      })
    );
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.API}/auth/register`, data);
  }

  logout(): void {
    const refreshToken = this.getStoredRefreshToken();
    this.http.post(
      `${this.API}/auth/logout`,
      { refreshToken },
      { withCredentials: true }
    ).subscribe({ error: () => {} }); // Fire and forget

    this.clearAuth();
    this.router.navigate(['/auth/login']);
  }

  private _refreshing$: Observable<LoginResponse> | null = null;

  refreshToken(): Observable<LoginResponse> {
    if (this._refreshing$) {
      return this._refreshing$;
    }

    this._refreshing$ = this.http.post<LoginResponse>(
      `${this.API}/auth/refresh`,
      {},
      { withCredentials: true }
    ).pipe(
      tap(res => this.handleAuthSuccess(res)),
      catchError(err => {
        // JANGAN clearAuth() dan JANGAN redirect di sini
        // Biarkan caller yang memutuskan
        return throwError(() => err);
      }),
      finalize(() => {
        this._refreshing$ = null;
      }),
      shareReplay(1)
    );

    return this._refreshing$;
  }

  private handleAuthSuccess(res: LoginResponse): void {
    this._accessToken.set(res.accessToken);
    const expiry = new Date(Date.now() + res.expiresIn * 1000);
    this._tokenExpiry.set(expiry);

    if (res.user) {
      this._currentUser.set(res.user);
      localStorage.setItem('ft_user', JSON.stringify(res.user));
    }

    // Simpan expiry untuk restore session
    localStorage.setItem('ft_token_expiry', expiry.toISOString());

    // Auto-refresh 1 menit sebelum expire
    this.scheduleRefresh(res.expiresIn);
  }

  private scheduleRefresh(expiresIn: number): void {
    if (this._refreshTimer) clearTimeout(this._refreshTimer);
    const refreshIn = Math.max((expiresIn - 60) * 1000, 0);
    this._refreshTimer = setTimeout(() => {
      this.refreshToken().subscribe();
    }, refreshIn);
  }

  private restoreSession(): void {
    const storedUser = localStorage.getItem('ft_user');
    const storedExpiry = localStorage.getItem('ft_token_expiry');

    if (!storedUser || !storedExpiry) return;

    const expiry = new Date(storedExpiry);
    const now = new Date();

    if (expiry > now) {
      // Restore user state dulu SEBELUM refresh
      this._currentUser.set(JSON.parse(storedUser));
      this._tokenExpiry.set(expiry);

      // Refresh token di background — kalau gagal JANGAN redirect
      this.refreshToken().subscribe({
        error: () => {
          // Gagal refresh tapi JANGAN logout — biarkan user tetap di halaman
          // Hanya clear kalau memang token sudah benar-benar expired
          console.warn('Silent refresh failed, user stays logged in');
        }
      });
    }
    // Kalau expired, diam saja — biarkan guard yang handle redirect
  }

  private clearAuth(): void {
    if (this._refreshTimer) clearTimeout(this._refreshTimer);
    this._accessToken.set(null);
    this._tokenExpiry.set(null);
    this._currentUser.set(null);
    localStorage.removeItem('ft_user');
    localStorage.removeItem('ft_token_expiry');
  }

  private getStoredRefreshToken(): string | null {
    // Refresh token ada di HttpOnly cookie, tidak bisa dibaca JS
    // Backend baca dari cookie langsung
    return null;
  }
}