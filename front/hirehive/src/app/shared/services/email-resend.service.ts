import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '@shared/services/auth.service';
import { EmailConfirmationResendPayload } from 'src/app/client/models/auth-client.model';

export type ResendStatus = 'idle' | 'sending' | 'sent' | 'error';

@Injectable({ providedIn: 'root' })
export class EmailResendService {
  resendStatus$ = new BehaviorSubject<ResendStatus>('idle');

  constructor(private authService: AuthService) {}

  resendEmailConfirmation(email: string) {
    this.resendStatus$.next('sending');
    const emailResendData: EmailConfirmationResendPayload = {
      email: email
    }
    this.authService.resendConfirmation(emailResendData).subscribe({
      next: () => this.resendStatus$.next('sent'),
      error: () => this.resendStatus$.next('error'),
    });
  }
}
