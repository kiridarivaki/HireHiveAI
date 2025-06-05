import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AppFooterComponent } from '@shared/components/footer/footer.component';
import { AppDesktopNavComponent } from '@shared/components/navbar/desktop-nav/desktop-nav.component';
import { AppMobileNavComponent } from '@shared/components/navbar/compact-nav/compact-nav.component';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-layout',
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
  imports:[AppFooterComponent, AppDesktopNavComponent, AppMobileNavComponent, CommonModule, RouterOutlet]
})
export class MainLayoutComponent implements OnInit{
  isHomePage: boolean = false;
  compactMenu: boolean = false;

  constructor(
    private router: Router,
    private breakpointObserver: BreakpointObserver
  ) {
    this.router.events.subscribe(() => {
      this.isHomePage = this.router.url === '/' || this.router.url === '/home';
    });
  }
  ngOnInit(): void {
    this.breakpointObserver
    .observe(['(max-width: 768px)'])
    .subscribe(result => {
      this.compactMenu = result.matches;
    });
  }
}
