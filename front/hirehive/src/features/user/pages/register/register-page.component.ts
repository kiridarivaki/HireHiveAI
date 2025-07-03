import { trigger, transition, query, style, stagger, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmploymentStatus } from '@shared/constants/employment-options';
import { JobType } from '@shared/constants/job-types';
import { UserRole } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';
import { AuthService } from '@shared/services/auth.service';
import { ErrorService } from '@shared/services/error.service';
import { NotificationService } from '@shared/services/notification.service';
import { StorageService } from '@shared/services/storage.service';
import { fieldsMatchValidator } from '@shared/validators/fields-match.validator';
import { passwordValidator } from '@shared/validators/password.validator';
import { switchMap, tap } from 'rxjs';
import { RegisterPayload, RegisterResponse } from 'src/app/client/models/auth-client.model';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
  animations:  [ 
    trigger('fadeInOnly', [
    transition(':enter', [
      style({ opacity: 0 }),
      animate('1500ms  ease-out', style({ opacity: 1 }))
    ])
  ])]
})
export class RegisterPageComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private storageService: StorageService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  employmentOptions = Object.entries(EmploymentStatus).map(([key, label], index) => ({
    value: index.toString(),  
    label: label as string
  }));
  jobTypes = Object.entries(JobType).map(([key, label], index) => ({
    value: index.toString(),  
    label: label as string
  }));

  registerForm = new FormGroup(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, passwordValidator]),
      confirmPassword: new FormControl('', [Validators.required, passwordValidator]),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      employmentStatus: new FormControl<EmploymentStatus>(EmploymentStatus.full_time, [Validators.required]),
      jobTypes: new FormControl<JobType[] | null>([], Validators.required) 
    },
    { validators: fieldsMatchValidator('password', 'confirmPassword') }
  );

  ngOnInit(): void {
  }

  get passwordControl() {
    return this.registerForm.get('password');
  }

  get confirmPasswordControl() {
    return this.registerForm.get('confirmPassword');
  }

  onRegister() {
    if (this.registerForm.valid) {
      const registerForm = this.registerForm.value;

      const employmentStatusValue = Number(registerForm.employmentStatus) as unknown as EmploymentStatus;
      const jobTypesValue = (registerForm.jobTypes || []).map((x: string) => Number(x)) as unknown as JobType[];

      const registerData: RegisterPayload = {
        email: registerForm.email!,
        firstName: registerForm.firstName!,
        lastName: registerForm.lastName!,
        employmentStatus: employmentStatusValue!,
        jobTypes: jobTypesValue!,
        password: registerForm.password!,
        confirmPassword: registerForm.confirmPassword!
      };

      let registeredUserId: string;

      this.authService.register(registerData)
        .pipe(
          tap((res: RegisterResponse) => {
            registeredUserId = res.userId;
            this.storageService.storeAuth({
              accessToken: res.accessToken,
              refreshToken: res.refreshToken,
              expiresIn: res.expiresIn
            });
          }),
          switchMap(() => this.authService.getUserInfo(registeredUserId))
        )
        .subscribe({
          next: (userInfo) => {
            const roles: UserRole[] = userInfo.roles
              .filter((role): role is UserRole =>
                Object.values(UserRole).includes(role as UserRole)
              );

            const user: User = {
              id: registeredUserId,
              roles: roles,
              email: userInfo.email,
              firstName: userInfo.firstName,
              lastName: userInfo.lastName,
              emailConfirmed: userInfo.emailConfirmed
            };

            this.storageService.setUser(user);
            this.authService.setUser(user);
            this.notificationService.showNotification('Registration successful. Welcome!');
            this.router.navigate([`/user/profile/${user.id}`]);
          },
          error: (err) => {
            this.storageService.removeAuth();
            this.storageService.removeUser();
            if (err.status === 409) {
              this.errorService.showError('A user with this email exists.');
              this.registerForm.reset({ jobTypes: [] });
            } else {
              this.errorService.showError('Registration failed. Please try again.');
            }
          }
        });
    }
  }
}
