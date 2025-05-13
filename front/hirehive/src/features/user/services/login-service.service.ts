import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { User } from "src/app/client/models/user.model";
import { AuthClientService } from "src/app/client/services/auth-client.service";

@Injectable({ providedIn: 'root' })
export class LoginService {
  constructor(private authService: AuthClientService, private router: Router) {}

  handleLogin(loginData: any): void {
    const userObj: User = {
      email: loginData.email,
      firstName: loginData.firstName,
      lastName: loginData.lastName,
      employmentStatus: loginData.employmentStatus,
      password: loginData.password,
      confirmPassword: loginData.confirmPassword
    };

    this.authService.login(userObj).pipe(
      tap(() => this.router.navigate(['/home']))
    );
  }
}