import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';

import { LoginPageComponent } from './pages/login/login-page.component'; 
import { SharedModule } from '@shared/shared.module';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ResumeUploadModule } from '../resume-upload/resume-upload.module';

@NgModule({
  declarations: [LoginPageComponent, RegisterPageComponent], 
  imports: [CommonModule, UserRoutingModule, SharedModule, ResumeUploadModule] 
})
export class UserModule {}
