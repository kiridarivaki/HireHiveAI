import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { UserRole } from "@shared/models/auth.model";
import { AuthService } from "@shared/services/auth.service";

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService, 
    private router: Router
) {}

  canActivate(): boolean {
    const user = this.authService.getCurrentUser()
    if (user?.roles.includes(UserRole.admin)){
        return true;
    } else {
        this.router.navigate(['/access-denied']);
        return false;
    }
  }
}