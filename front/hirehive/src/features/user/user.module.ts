import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';

import { LoginPageComponent } from './pages/login/login-page.component'; 
import { SharedModule } from '@shared/shared.module';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ResumeUploadModule } from '../resume-upload/resume-upload.module';
import { MatError, MatFormFieldModule } from '@angular/material/form-field';
import { ValidationErrorPipe } from "@shared/pipes/validation-error-pipe.pipe";
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { ProfilePageComponent } from './pages/profile/profile-page.component';
import {MatCardModule} from '@angular/material/card';


@NgModule({
  declarations: [LoginPageComponent, RegisterPageComponent, ProfilePageComponent], 
  imports: [CommonModule, UserRoutingModule, SharedModule, ResumeUploadModule, MatFormFieldModule, ValidationErrorPipe, MainLayoutComponent, MatCardModule, MatError] 
})
export class UserModule {}
