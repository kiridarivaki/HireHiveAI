import { Component } from '@angular/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { NotificationService } from '@shared/services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [MatSnackBarModule],
  template: ``,
})
export class AppNotificationComponent {
  constructor(
    private notificationService: NotificationService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.notificationService.notification$.subscribe(message => {
      if (message) {
        this.snackBar.open(message, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-success'] 
        });
      }
    });
  }
}
