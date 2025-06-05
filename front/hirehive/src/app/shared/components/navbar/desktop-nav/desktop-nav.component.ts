import { Component, OnInit } from '@angular/core';
import { UserRole } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';
import { AuthService } from '@shared/services/auth.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { DialogService } from '@shared/services/dialog.service';
import { ConfirmDialogComponent } from '../../dialog/confirm-dialog/confirm-dialog.component';

interface MenuOption {
  label: string;
  route: string;
}

@Component({
  selector: 'app-desktop-nav',
  templateUrl: './desktop-nav.component.html',
  styleUrl: './desktop-nav.component.scss',
  imports: [MatToolbarModule, RouterLink, CommonModule]
})
export class AppDesktopNavComponent implements OnInit {
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
    });
  }

  get isAdmin$(): Observable<boolean> {
    return this.authService.isAdmin$();
  }

  goHome() {
    this.router.navigate(['/home']);
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
