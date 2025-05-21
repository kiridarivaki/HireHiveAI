import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { ErrorService } from '@shared/services/error.service';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
  constructor(
    private router: Router, 
    private errorService: ErrorService
  ) {}

  intercept(request: HttpRequest<undefined>, next: HttpHandler): Observable<HttpEvent<undefined>> {
    if (this.shouldNotIntercept(request.url)) {
      return next.handle(request);
    } 
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        let message = 'Something went wrong. Please try again.'
        switch (err.status) {
          case 400:
            message = 'Bad request. Please check your input.';
            break;
          case 401:
            message = 'Unauthorized. Please log in again.';
            this.router.navigate(['/login']);
            break;
          case 403:
            message = 'Forbidden. You don’t have permission.';
            break;
          case 500:
            message = 'Server error. Try again later.';
            break;
          default:
            message = 'An unexpected error occurred.';
            break;
        }
        this.errorService.showError(message);

        return throwError(() => err);
      })
    );
  }

  private shouldNotIntercept(url: string): boolean {
    return (
      url.includes('/login') ||
      url.includes('/refreshToken') ||
      url.includes('/register')
    );
  }
} 
