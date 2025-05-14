import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { SharedModule } from '@shared/shared.module';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';

@NgModule({
  declarations: [HomeComponent],
  imports: [CommonModule, SharedModule, MainLayoutComponent],
})
export class HomeModule {}