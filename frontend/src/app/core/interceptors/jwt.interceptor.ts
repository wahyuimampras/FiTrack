import { HttpInterceptorFn, HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

let isRefreshing = false;

export const jwtInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // Skip auth endpoints (kecuali refresh — butuh cookie)
  if (req.url.includes('/auth/login') || req.url.includes('/auth/register')) {
    return next(req);
  }

  // Selalu tambahkan withCredentials agar cookie refreshToken ikut dikirim
  const withCreds = req.clone({ withCredentials: true });

  const addToken = (request: HttpRequest<unknown>) => {
    if (!auth.token) return request;
    return request.clone({
      withCredentials: true,
      setHeaders: { Authorization: `Bearer ${auth.token}` }
    });
  };

  return next(addToken(req)).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isRefreshing) {
        isRefreshing = true;
        return auth.refreshToken().pipe(
          switchMap(() => {
            isRefreshing = false;
            return next(addToken(req));
          }),
          catchError(refreshErr => {
            isRefreshing = false;
            router.navigate(['/auth/login']);
            return throwError(() => refreshErr);
          })
        );
      }
      return throwError(() => error);
    })
  );
};