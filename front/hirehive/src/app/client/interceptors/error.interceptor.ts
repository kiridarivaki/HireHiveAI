import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor{
  constructor(
    private router: Router, 
    private injector: Injector
  ) {}

  intercept(request: HttpRequest<undefined>, next: HttpHandler): Observable<HttpEvent<undefined>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        const snackBar = this.injector.get(MatSnackBar);
        const router = this.injector.get(Router);

        switch (err.status) {
          case 400:
            snackBar.open('Bad request. Please check your input.', 'Close', { duration: 3000 });
            break;
          case 401:
            snackBar.open('Unauthorized. Please log in again.', 'Close', { duration: 3000 });
            router.navigate(['/login']);
            break;
          case 403:
            snackBar.open('Forbidden. You don’t have permission.', 'Close', { duration: 3000 });
            break;
          case 404:
            snackBar.open('Resource not found.', 'Close', { duration: 3000 });
            break;
          case 500:
            snackBar.open('Server error. Try again later.', 'Close', { duration: 3000 });
            break;
          default:
            snackBar.open('An unexpected error occurred.', 'Close', { duration: 3000 });
            break;
        }
        return throwError(() => err);
      })
    );
  }
} 
