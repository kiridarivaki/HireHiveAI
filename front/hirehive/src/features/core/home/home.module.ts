import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { SharedModule } from '@shared/shared.module';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { MatCardModule } from '@angular/material/card';
import { AppButtonComponent } from '@shared/components/button/button.component';

@NgModule({
  declarations: [],
  imports: [CommonModule, SharedModule, MainLayoutComponent,MatCardModule, AppButtonComponent],
})
export class HomeModule {}