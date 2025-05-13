import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { fieldsMatchValidator } from '@shared/validators/fields-match.validator';
import { passwordValidator } from '@shared/validators/password.validator';
import { RegisterService } from '../../services/register-service.service';

@Component({
  selector: 'app-register-page',
  standalone: false,
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent implements OnInit {

  constructor(
    private registerService: RegisterService
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
      const userData = this.registerForm.value;
      this.registerService.handleRegister(userData);
    };
  }
}
