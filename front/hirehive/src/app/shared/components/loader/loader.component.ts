import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoaderService } from '@shared/services/loader.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css'],
  imports: [MatProgressSpinnerModule, CommonModule]
})
export class LoaderComponent implements OnInit {
  loading$!: Observable<boolean>;

  constructor(private loaderService: LoaderService) {}

  ngOnInit(): void {
    this.loading$ = this.loaderService.loading$;
    this.loading$.subscribe();
  }
}