import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { EmploymentStatus } from '@shared/constants/employment-options';
import { AuthService } from '@shared/services/auth.service';
import { fieldsMatchValidator } from '@shared/validators/fields-match.validator';
import { passwordValidator } from '@shared/validators/password.validator';
import { RegisterPayload } from 'src/app/client/models/auth-client.model';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  employmentOptions: { value: string, label: string }[] = [];

  registerForm = new FormGroup(
    {
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, passwordValidator]),
      confirmPassword: new FormControl('', [Validators.required]),
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      employmentStatus: new FormControl('Employed', Validators.required)
    },
    { validators: fieldsMatchValidator('password', 'confirmPassword') }
  );

  ngOnInit(): void {
    this.employmentOptions = Object.values(EmploymentStatus).map(status => ({
      value: status,
      label: status
    }));
  }

  onRegister(){
    if (this.registerForm.valid) {
      const registerForm = this.registerForm.value;
      
      const registerData: RegisterPayload = {
        email: registerForm.email!,
        firstName: registerForm.firstName!,
        lastName: registerForm.lastName!,
        employmentStatus: registerForm.employmentStatus!,
        password: registerForm.password!,
        confirmPassword: registerForm.confirmPassword!
      };

      this.authService.register(registerData).subscribe({
        next: () => {
          this.router.navigate(['/check-email'], { queryParams: { email: registerData.email } });
        },
        error: (err) => {
          console.error('Registration failed', err);
        }
      });
    };
  }
}
