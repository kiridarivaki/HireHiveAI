import { Component, OnInit } from '@angular/core';
import { UserRole } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';
import { AuthService } from '@shared/services/auth.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AppButtonComponent } from '../button/button.component';
import { CommonModule } from '@angular/common';

interface MenuOption {
  label: string;
  route: string;
}

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
  imports: [MatToolbarModule, AppButtonComponent, CommonModule]
})
export class AppNavBarComponent implements OnInit {
  UserRole = UserRole;
  currentUser: User | null = null;
  menuOptions: MenuOption[] = [];
  isLoggedIn = false;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isLoggedIn = !!user;
      this.setupMenuOptions();
    });
  }

  hasRole(role: UserRole): boolean {
    return !!this.currentUser?.roles?.includes(role);
  }

  setupMenuOptions() {
    this.menuOptions = [
      { label: 'Home', route: '/' },
      { label: 'About Us', route: '/about' }
    ];

    if (!this.isLoggedIn) {
      this.menuOptions.push(
        { label: 'Register', route: '/register' },
        { label: 'Login', route: '/login' }
      );
    } else {
      // Logged-in user menu
      this.menuOptions.push(
        { label: 'Profile', route: '/profile' },
        { label: 'Logout', route: '/logout' }
      );

      if (this.hasRole(UserRole.admin)) {
        this.menuOptions.push(
          { label: 'Find Candidate Matches', route: '/matches' }
        );
      }
    }
  }

  onLogout() {
    this.authService.logout();
    // Optionally redirect after logout
  }
}
