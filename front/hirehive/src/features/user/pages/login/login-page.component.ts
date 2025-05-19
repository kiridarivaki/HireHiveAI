import { Component } from '@angular/core';
import { FormGroup, FormControl,  Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { passwordValidator } from '@shared/validators/password.validator';
import { LoginPayload } from 'src/app/client/models/auth-client.model';
import { AuthClientService } from 'src/app/client/services/auth-client.service';

@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
    constructor(
      private authService: AuthClientService,
      private router: Router
    ) {}
  loginForm = new FormGroup ({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, passwordValidator]),
  });

  onLogin() { 
    if (this.loginForm.valid) {
        const loginForm = this.loginForm.value;

        const loginData: LoginPayload = {
          email: loginForm.email!,
          password: loginForm.password!
        };

        this.authService.login(loginData).subscribe({
            next: (response) => {
              this.router.navigate([`/user/${response.userId}`]);
            },
            error: (err)=>{
              this.router.navigate(['/home']);
            }
          }
        )
    }
  }
}