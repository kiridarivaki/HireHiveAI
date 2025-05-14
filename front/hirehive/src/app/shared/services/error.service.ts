import { Injectable, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  private errorSubject = new Subject<string | null>(); 
  error$ = this.errorSubject.asObservable();

  showError(message: string): void {
    this.errorSubject.next(message); 
  }

  hideError(): void {
    this.errorSubject.next(null);
  }
}