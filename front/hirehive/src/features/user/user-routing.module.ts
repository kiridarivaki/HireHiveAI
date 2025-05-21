import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login/login-page.component';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ProfilePageComponent } from './pages/profile/profile-page.component';
import { AuthGuard } from '@shared/guards/auth.guard';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';

const routes: Routes = [{
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'user/:userId', component: ProfilePageComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginPageComponent },
      { path: 'register', component: RegisterPageComponent }
    ]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule {}
