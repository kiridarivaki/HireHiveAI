import { Injectable, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notificationSubject = new Subject<string | null>(); 
  notification$ = this.notificationSubject.asObservable();

  showNotification(message: string): void {
    this.notificationSubject.next(message); 
  }

  hideNotification(): void {
    this.notificationSubject.next(null);
  }
}