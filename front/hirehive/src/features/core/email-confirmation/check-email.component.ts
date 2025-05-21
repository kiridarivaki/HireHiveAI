import { CommonModule } from "@angular/common";
import { Component, OnDestroy } from "@angular/core";
import { MatIconModule } from "@angular/material/icon";
import { ActivatedRoute, Router } from "@angular/router";
import { EmailResendService, ResendStatus } from "@shared/services/email-resend.service";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-check-email',
  templateUrl: './check-email.component.html',
  styleUrls: ['./check-email.component.css'],
  imports: [MatIconModule, CommonModule]
})
export class CheckEmailComponent implements OnDestroy {
  userEmail: string | null = null;
  resendStatus: ResendStatus = 'idle';
  private resendSub?: Subscription;

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

  resendEmailConfirmation(): void {
    if (!this.userEmail) return;
    this.resendService.resendEmailConfirmation(this.userEmail);
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  ngOnDestroy(): void {
    this.resendSub?.unsubscribe();
  }
}