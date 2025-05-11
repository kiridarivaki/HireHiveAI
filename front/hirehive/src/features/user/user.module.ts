import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';

import { LoginPageComponent } from './pages/login/login-page.component'; 
import { SharedModule } from '@shared/shared.module';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ResumeUploadModule } from '../resume-upload/resume-upload.module';

@NgModule({
  declarations: [LoginPageComponent, RegisterPageComponent], 
  imports: [CommonModule, UserRoutingModule, SharedModule, ResumeUploadModule, MatInputModule, MatSelectModule, MatIconModule] 
})
export class UserModule {}
