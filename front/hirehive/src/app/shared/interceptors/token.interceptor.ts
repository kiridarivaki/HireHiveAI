import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@shared/services/auth.service';
import { StorageService } from '@shared/services/storage.service';
import { Observable, switchMap } from 'rxjs';

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
  private isRefreshing = false;
  constructor(
    private storageService: StorageService,
    private authService: AuthService
  ){}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.shouldNotIntercept(req.url)){
      return next.handle(req);
    }
    const isExpired = this.authService.isTokenExpired();
    if (isExpired){
      return this.handleTokenRefresh(req, next);
    }
    const authRequest = this.addTokenToRequest(req);
    return next.handle(authRequest);
  }

  handleTokenRefresh(request: HttpRequest<any>, next: HttpHandler){
    if (!this.isRefreshing){
      this.isRefreshing = true;
      if (this.authService.isLoggedIn()){
          return this.authService.refreshToken().pipe(
          switchMap(() => {
            this.isRefreshing = false;
            return next.handle(request);
          }))
      }
    }
    return next.handle(request);
  }

  addTokenToRequest(request: HttpRequest<any>): HttpRequest<any>{
    const token = this.storageService.getAuth()?.accessToken;
    const authRequest = request.clone({
      setHeaders: {Authorization: `Bearer ${token}`}
    });
    return authRequest;
  }

  shouldNotIntercept(url: string) {
      return (
          url.includes('/login') ||
          url.includes('/refreshToken') ||
          url.includes('/register')
      );
  }
}
