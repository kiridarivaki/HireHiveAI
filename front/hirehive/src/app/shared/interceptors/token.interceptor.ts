import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';

import { AuthService } from '@shared/services/auth.service';
import { StorageService } from '@shared/services/storage.service';
import { RefreshTokenPayload } from 'src/app/client/models/auth-client.model';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  constructor(
    private storageService: StorageService,
    private authService: AuthService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.shouldNotIntercept(req.url)) {
      return next.handle(req);
    }

    if (this.authService.isTokenExpired()) {
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
      const currentRefreshToken: RefreshTokenPayload = {
        accessToken: storedAuth?.refreshToken || '',
        refreshToken: storedAuth?.refreshToken || ''
      };

      if (this.authService.isLoggedIn() && currentRefreshToken.refreshToken) {
        return this.authService.refreshToken(currentRefreshToken).pipe(
          switchMap(newAuth => {
            this.isRefreshing = false;
            this.storageService.storeAuth(newAuth);
            this.refreshTokenSubject.next(newAuth.accessToken);

            const authRequest = this.addTokenToRequest(request);
            return next.handle(authRequest);
          }),
          catchError(err => {
            this.isRefreshing = false;
            this.storageService.removeAuth();
            this.refreshTokenSubject.next(null);

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
