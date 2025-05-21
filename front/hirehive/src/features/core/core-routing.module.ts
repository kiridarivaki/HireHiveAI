import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { CheckEmailComponent } from './email-confirmation/check-email.component';

const routes: Routes = [
    { path: 'confirm-email', component: EmailConfirmationComponent },
    { path: 'check-email', component: CheckEmailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule {}
