import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (req.url.includes('/auth/')) return next(req);

  const addToken = (r: typeof req) =>
    auth.token ? r.clone({ setHeaders: { Authorization: `Bearer ${auth.token}` } }) : r;

  return next(addToken(req)).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return auth.refreshToken().pipe(
          switchMap(() => {
            return next(addToken(req));
          }),
          catchError(refreshErr => {
            // Hanya redirect ke login kalau refresh benar-benar gagal
            // (bukan karena hot reload)
            router.navigate(['/auth/login']);
            return throwError(() => refreshErr);
          })
        );
      }
      return throwError(() => error);
    })
  );
};