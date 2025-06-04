import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppButtonComponent } from '@shared/components/button/button.component';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
  imports: [AppButtonComponent]
})
export class NotFoundComponent {
  constructor(private router: Router) {}

  goHome(): void {
    this.router.navigate(['/']);
  }
}
