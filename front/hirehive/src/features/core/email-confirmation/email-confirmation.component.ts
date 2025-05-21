import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { AuthService } from '@shared/services/auth.service';
import { tap } from 'rxjs';
import { EmailConfirmationPayload } from 'src/app/client/models/auth-client.model';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, AppButtonComponent],
  styleUrl: './email-confirmation.component.css'
})
export class EmailConfirmationComponent implements OnInit{
  private userEmail: string | null = null;
  confirmationStatus: 'pending' | 'success' | 'error'| 'alreadyConfirmed' | 'tokenExpired' = 'pending';
  resendStatus: 'idle' | 'sending' | 'sent' | 'error' = 'idle';

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const userId = this.route.snapshot.queryParamMap.get('userId');
    const token = this.route.snapshot.queryParamMap.get('token');

    if (userId && token) {
      this.confirmationStatus = 'pending';

      this.authService.getUserInfo(userId).subscribe({
        next: (user) => {
          this.userEmail = user.email;
          if (user.isEmailConfirmed) {
            this.confirmationStatus = 'alreadyConfirmed';
          } else {
            const emailConfirmationData: EmailConfirmationPayload = {
              confirmationToken: token,
              email: user.email
            };

            this.authService.confirmEmail(emailConfirmationData).subscribe({
              next: () => {
                this.confirmationStatus = 'success';
              },
              error: () => {
                this.confirmationStatus = 'error';
              }
            });
          }
        },
        error: (err) => {
          if (err.status === 400 && err.error?.message === 'Token Expired') {
            this.confirmationStatus = 'tokenExpired';
          } else {
            this.confirmationStatus = 'error';
          }
        }
      });
    } else {
      this.confirmationStatus = 'error';
    }
  }

  resendEmailConfirmation(): void {
    if (!this.userEmail) return;

    this.resendStatus = 'sending';
    this.authService.resendConfirmation(this.userEmail).subscribe({
      next: () => {
        this.resendStatus = 'sent';
      },
      error: () => {
        this.resendStatus = 'error';
      }
    });
  }
}