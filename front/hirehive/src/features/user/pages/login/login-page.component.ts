import { Component } from '@angular/core';
import { FormGroup, FormControl,  Validators } from '@angular/forms';

@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  loginForm = new FormGroup ({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  });

  onSubmit() { //todo: post to api
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
    }
  }
}