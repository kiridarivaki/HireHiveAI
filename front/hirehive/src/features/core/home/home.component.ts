import {
  Component,
  HostListener,
  OnInit,
  ElementRef,
  Renderer2,
} from '@angular/core';
import {
  trigger,
  style,
  animate,
  transition,
  state,
} from '@angular/animations';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { SharedModule } from '@shared/shared.module';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [CommonModule, SharedModule, MatCardModule, AppButtonComponent],
  animations: [
    trigger('fadeInUp', [
      state('hidden', style({ opacity: 0, transform: 'translateY(30px)' })),
      state('visible', style({ opacity: 1, transform: 'translateY(0)' })),
      transition('hidden => visible', animate('700ms ease-out')),
    ]),
  ],
})
export class HomeComponent implements OnInit {
  cardStates: string[] = [];

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngOnInit(): void {
    this.cardStates = ['hidden', 'hidden', 'hidden'];
  }

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    const cards = this.el.nativeElement.querySelectorAll('.step');
    cards.forEach((card: any, index: number) => {
      const rect = card.getBoundingClientRect();
      const inView = rect.top < window.innerHeight - 100;
      if (inView && this.cardStates[index] === 'hidden') {
        this.cardStates[index] = 'visible';
      }
    });
  }
}
