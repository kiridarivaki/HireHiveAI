import { Component } from '@angular/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ErrorService } from '@shared/services/error.service';

@Component({
  selector: 'app-error',
  imports: [MatSnackBarModule],
  templateUrl: './error.component.html',
})
export class ErrorComponent {
  constructor(
    private errorService: ErrorService,
    private snackBar: MatSnackBar
  ) {}

    ngOnInit(): void {
    this.errorService.error$.subscribe(message => {
      if (message) {
        this.snackBar.open(message, 'Close', { duration: 3000 });
      }
    });
  }
}
