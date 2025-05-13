import { Component } from '@angular/core';
import { FormGroup, FormControl,  Validators } from '@angular/forms';
import { passwordValidator } from '@shared/validators/password.validator';
import { LoginService } from '../../services/login-service.service';

@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html',
  //styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
    constructor(
      private loginService: LoginService
    ) {}
  loginForm = new FormGroup ({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, passwordValidator]),
  });

  onLogin() { 
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
      
    }
  }
}