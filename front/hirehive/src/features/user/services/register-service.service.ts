import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { User } from "src/app/client/models/user.model";
import { AuthClientService } from "src/app/client/services/auth-client.service";

@Injectable({ providedIn: 'root' })
export class RegisterService {
  constructor(private authService: AuthClientService, private router: Router) {}

  handleRegister(registerData: any): void {
    const userObj: User = {
      email: registerData.email,
      firstName: registerData.firstName,
      lastName: registerData.lastName,
      employmentStatus: registerData.employmentStatus,
      password: registerData.password,
      confirmPassword: registerData.confirmPassword
    };

    this.authService.register(userObj).pipe(
      tap(() => this.router.navigate(['/home']))
    );
  }
}