import { Component } from '@angular/core';
import { FormGroup, FormControl,  Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StorageService } from '@shared/services/storage.service';
import { passwordValidator } from '@shared/validators/password.validator';
import { tap } from 'rxjs';
import { LoginPayload } from 'src/app/client/models/auth-client.model';
import { StoredAuth, UserRole } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';
import { AuthService } from '@shared/services/auth.service';
import { ErrorService } from '@shared/services/error.service';
import { NotificationService } from '@shared/services/notification.service';
import { trigger, transition, style, animate, query, stagger } from '@angular/animations';


@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
  animations: [
    trigger('fadeInOnly', [
    transition(':enter', [
      style({ opacity: 0 }),
      animate('1500ms  ease-out', style({ opacity: 1 }))
    ])
  ]),
  trigger('formStagger', [
    transition(':enter', [
      query('.form-field', [
        style({ opacity: 0 }),
        stagger(150, [
          animate('0.5s 500ms  ease-out', style({ opacity: 1 }))
        ])
      ])
    ])
  ]),
  trigger('slideInLeft', [
    transition(':enter', [
      style({ transform: 'translateX(-50px)', opacity: 0 }),
      animate('1500ms  ease-out', style({ transform: 'translateX(0)', opacity: 1 }))
    ])
  ])
  ]
})
export class LoginPageComponent {
    constructor(
      private authService: AuthService,
      private storageService: StorageService,
      private notificationService: NotificationService,
      private errorService: ErrorService,
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

        this.authService.login(loginData)
        .pipe(
          tap((auth)=>{
            const storedAuth: StoredAuth = {
              accessToken: auth.accessToken,
              refreshToken: auth.refreshToken,
              expiresIn: auth.expiresIn
            }
            this.storageService.storeAuth(storedAuth);
          })
        )
        .subscribe({
            next: (response) => {
              this.fetchUser(response.userId)
            },
            error: (err)=>{
              this.storageService.removeAuth()
              this.storageService.removeUser()
              if (err.status === 401){
                this.errorService.showError('Invalid credentials.');
                this.loginForm.reset();
              }
            }
          }
        )
    }
  }

  fetchUser(userId: string){
    this.authService.getUserInfo(userId)
    .subscribe({
      next: (userInfo) => {
        const roles: UserRole[] = userInfo.roles
          .filter((role): role is UserRole =>
            Object.values(UserRole).includes(role as UserRole)
          );

        const user: User = {
          id: userId,
          roles: roles,
          email: userInfo.email,
          firstName: userInfo.firstName,
          lastName: userInfo.lastName,
          emailConfirmed: userInfo.emailConfirmed
        }

        this.storageService.setUser(user)
        this.authService.setUser(user)
        this.notificationService.showNotification('Successful login. Welcome!')
        this.router.navigate([`/user/profile/${userId}`]);
      }
    });
  }
}