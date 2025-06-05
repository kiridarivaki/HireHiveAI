import { trigger, transition, query, style, stagger, animate } from '@angular/animations';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmploymentStatus } from '@shared/constants/employment-options';
import { JobType } from '@shared/constants/job-types';
import { AuthService } from '@shared/services/auth.service';
import { ErrorService } from '@shared/services/error.service';
import { fieldsMatchValidator } from '@shared/validators/fields-match.validator';
import { passwordValidator } from '@shared/validators/password.validator';
import { RegisterPayload } from 'src/app/client/models/auth-client.model';

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

  onRegister(){
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

      this.authService.register(registerData).subscribe({
        next: () => {
          this.router.navigate(['/check-email'], { queryParams: { email: registerData.email } });
        },
        error: (err) => {
          if (err.status === 409){
            this.errorService.showError('A user with this email exists.');
            this.registerForm.reset({ jobTypes: [] });
          }
        }
      });
    };
  }
}
