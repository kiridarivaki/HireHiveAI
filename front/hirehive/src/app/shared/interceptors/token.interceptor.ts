import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, switchMap, take, tap } from 'rxjs/operators';

import { AuthService } from '@shared/services/auth.service';
import { StorageService } from '@shared/services/storage.service';
import { RefreshTokenPayload } from 'src/app/client/models/auth-client.model';
import { ErrorService } from '@shared/services/error.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(
    private storageService: StorageService,
    private authService: AuthService,
    private errorService: ErrorService 
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.shouldNotIntercept(req.url)) {
      return next.handle(req);
    }

    const tokenExpired = this.authService.isTokenExpired();
    if (tokenExpired) {
      return this.handleTokenRefresh(req, next);
    }

    const authRequest = this.addTokenToRequest(req);
    return next.handle(authRequest);
  }

  private handleTokenRefresh(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const storedAuth = this.storageService.getAuth();

      const accessToken = storedAuth?.accessToken;
      const refreshToken = storedAuth?.refreshToken;

      if (this.authService.isLoggedIn() && refreshToken) {
        const refreshData: RefreshTokenPayload = { 
          accessToken: accessToken,
          refreshToken: refreshToken 
        };

        return this.authService.refreshToken(refreshData).pipe(
          tap((newAuth) => {
            const expiresAt = Date.now() + newAuth.expiresIn * 1000;
            this.storageService.storeAuth({
              accessToken: newAuth.accessToken,
              refreshToken: newAuth.refreshToken,
              expiresIn: expiresAt
            });
          }),
          switchMap((newAuth) => {
            this.isRefreshing = false;
            this.refreshTokenSubject.next(newAuth.accessToken);

            const authRequest = this.addTokenToRequest(request);
            return next.handle(authRequest);
          }),
          catchError(err => {
            this.isRefreshing = false;
            this.refreshTokenSubject.next(null);
            this.authService.logout();
            this.errorService.showError('Your session has expired. Please log in again.');

            return throwError(() => err);
          })
        );
      } else {
        this.isRefreshing = false;
        this.storageService.removeAuth();
      }
    }

    return this.refreshTokenSubject.pipe(
      filter(token => token != null),
      take(1),
      switchMap(() => {
        const authRequest = this.addTokenToRequest(request);
        return next.handle(authRequest);
      })
    );
  }

  private addTokenToRequest(request: HttpRequest<any>): HttpRequest<any> {
    const token = this.storageService.getAuth()?.accessToken;
    if (!token) return request;

    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private shouldNotIntercept(url: string): boolean {
    return (
      url.includes('/login') ||
      url.includes('/refresh') ||
      url.includes('/register')
    );
  }
}
