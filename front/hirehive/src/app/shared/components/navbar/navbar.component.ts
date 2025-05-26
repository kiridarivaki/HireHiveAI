import { Component, OnInit } from '@angular/core';
import { UserRole } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';
import { AuthService } from '@shared/services/auth.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AppButtonComponent } from '../button/button.component';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DialogService } from '@shared/services/dialog.service';
import { ConfirmDialogComponent } from '../dialog/confirm-dialog/confirm-dialog.component';

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

  constructor(
    private authService: AuthService,
    private dialogService: DialogService, 
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isLoggedIn = !!user;
      this.setupMenuOptions();
    });
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
      this.menuOptions.push(
        { label: 'Profile', route: '/profile' },
        { label: 'Logout', route: '/logout' }
      );
      if (this.authService.isAdmin$()) {
        this.menuOptions.push({ label: 'Find Candidate Matches', route: '/matches' });
      }
    }
  }

  get isAdmin$(): Observable<boolean> {
    return this.authService.isAdmin$();
  }

  goHome() {
    this.router.navigate(['/']);
  }

  onLogout() {
    this.dialogService.open(ConfirmDialogComponent, {
      title: 'Confirm Logout',
    })
    .afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }
      this.authService.logout();
      this.router.navigate(['/home']);
    });
  }
}
