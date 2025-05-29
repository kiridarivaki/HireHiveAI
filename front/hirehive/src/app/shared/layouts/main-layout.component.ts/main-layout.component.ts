import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AppFooterComponent } from '@shared/components/footer/footer.component';
import { AppNavBarComponent } from '@shared/components/navbar/navbar.component';

@Component({
  selector: 'app-layout',
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
  imports:[AppFooterComponent, AppNavBarComponent, CommonModule, RouterOutlet]
})
export class MainLayoutComponent {
  isHomePage: boolean = false;

  constructor(private router: Router) {
    this.router.events.subscribe(() => {
      this.isHomePage = this.router.url === '/' || this.router.url === '/home';
    });
  }
}
