import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';

import { LoginPageComponent } from './pages/login/login-page.component'; 
import { SharedModule } from '@shared/shared.module';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ResumeUploadModule } from '../resume-upload/resume-upload.module';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ValidationErrorPipe } from "@shared/pipes/validation-error-pipe.pipe";

@NgModule({
  declarations: [LoginPageComponent, RegisterPageComponent], 
  imports: [CommonModule, UserRoutingModule, SharedModule, ResumeUploadModule, MatFormFieldModule, ValidationErrorPipe] 
})
export class UserModule {}
