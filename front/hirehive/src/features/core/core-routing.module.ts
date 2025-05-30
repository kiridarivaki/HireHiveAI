import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { CheckEmailComponent } from './email-confirmation/check-email.component';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [{
      path: '',
      component: MainLayoutComponent,
      children: [
        { path: 'home', component: HomeComponent },
        { path: '', component: HomeComponent }
      ]},
    { path: 'confirm-email/:userEmail/:token', component: EmailConfirmationComponent },
    { path: 'check-email', component: CheckEmailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule {}
