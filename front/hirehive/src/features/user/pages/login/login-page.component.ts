import { Component } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html'
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