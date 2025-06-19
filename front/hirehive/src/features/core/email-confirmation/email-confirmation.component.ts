import { CommonModule } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { AuthService } from '@shared/services/auth.service';
import { EmailResendService, ResendStatus } from '@shared/services/email-resend.service';
import { EmailConfirmationPayload } from 'src/app/client/models/auth-client.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css'],
  standalone: true,
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, AppButtonComponent]
})
export class EmailConfirmationComponent implements OnInit, OnDestroy {
  private userEmail: string | null = null;
  confirmationStatus: 'pending' | 'success' | 'error' | 'alreadyConfirmed' = 'pending';
  resendStatus: ResendStatus = 'idle';
  private resendSub?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private resendService: EmailResendService
  ) {}

  ngOnInit(): void {
    this.resendSub = this.resendService.resendStatus$.subscribe(status => {
      this.resendStatus = status;
  });

    const userEmail = this.route.snapshot.paramMap.get('userEmail');
    const token = this.route.snapshot.paramMap.get('token');

    if (token && userEmail) {
      this.confirmationStatus = 'pending';
      const emailConfirmationData: EmailConfirmationPayload = {
        confirmationToken: token,
        email: userEmail
      };
      this.authService.confirmEmail(emailConfirmationData).subscribe({
        next: () => {
          this.confirmationStatus = 'success';
        },
        error: () => {
          this.confirmationStatus = 'error';
        }
      });
    } else {
      this.confirmationStatus = 'error';
    }
  }

  resendEmailConfirmation(): void {
    if (!this.userEmail) return;
    this.resendService.resendEmailConfirmation(this.userEmail);
  }

  ngOnDestroy(): void {
    this.resendSub?.unsubscribe();
  }
}
