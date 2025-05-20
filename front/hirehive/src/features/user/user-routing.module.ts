import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login/login-page.component';
import { RegisterPageComponent } from './pages/register/register-page.component';
import { ProfilePageComponent } from './pages/profile/profile-page.component';
import { AuthGuard } from '@shared/guards/auth.guard';

const routes: Routes = [
    { path: 'user/:userId', component: ProfilePageComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule {}
