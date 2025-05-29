import { CommonModule } from "@angular/common";
import { Component, OnDestroy } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { MatIconModule } from "@angular/material/icon";
import { ActivatedRoute, Router } from "@angular/router";
import { AppButtonComponent } from "@shared/components/button/button.component";
import { AppInputComponent } from "@shared/components/input/input.component";
import { EmailResendService, ResendStatus } from "@shared/services/email-resend.service";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-check-email',
  templateUrl: './check-email.component.html',
  styleUrls: ['./email-confirmation.component.css'],
  imports: [MatIconModule, CommonModule, AppButtonComponent, AppInputComponent, AppButtonComponent, FormsModule]
})
export class CheckEmailComponent implements OnDestroy {
  userEmail: string | null = null;
  resendStatus: ResendStatus = 'idle';
  private resendSub?: Subscription;
  tokenInput: string = '';

  constructor(
    private route: ActivatedRoute,
    private resendService: EmailResendService,
    private router: Router
  ) {
    this.userEmail = this.route.snapshot.queryParamMap.get('email');
    this.resendSub = this.resendService.resendStatus$.subscribe(status => {
      this.resendStatus = status;
    });
  }

  goToConfirmation(): void {
    if (!this.tokenInput.trim()) return;

    this.router.navigate(['/confirm-email', this.userEmail, this.tokenInput]);
  }

  resendEmailConfirmation(): void {
    if (!this.userEmail) return;
    this.resendService.resendEmailConfirmation(this.userEmail);
  }

  ngOnDestroy(): void {
    this.resendSub?.unsubscribe();
  }
}