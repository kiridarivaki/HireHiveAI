import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { fieldsMatchValidator } from '@shared/validators/fields-match.validator';
import { passwordValidator } from '@shared/validators/password.validator';
import { RegisterFormParameters } from 'src/app/client/models/auth-client.model';
import { AuthClientService } from 'src/app/client/services/auth-client.service';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent implements OnInit {

  constructor(
    private authService: AuthClientService
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

  // constants file ?
  ngOnInit(): void {
    this.employmentOptions = [
      { value: 'Employed', label: 'Employed' },
      { value: 'Unemployed', label: 'Unemployed' },
      { value: 'Student', label: 'Student' }
    ];
  }

  onRegister(){
    if (this.registerForm.valid) {
      const registerForm = this.registerForm.value;
      
      const registerData: RegisterFormParameters = {
        email: registerForm.email!,
        firstName: registerForm.firstName!,
        lastName: registerForm.lastName!,
        employmentStatus: registerForm.employmentStatus!,
        password: registerForm.password!,
        confirmPassword: registerForm.confirmPassword!
      };

      this.authService.register(registerData);
    };
  }
}
