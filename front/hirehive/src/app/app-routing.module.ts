import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { CoreRoutingModule } from 'src/features/core/core-routing.module';

const routes: Routes = [
  { path: 'user', loadChildren: () => import('../features/user/user.module').then(m => m.UserModule) },
  { path: 'admin', loadChildren: () => import('../features/admin/admin.module').then(m => m.AdminModule) },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes), CoreRoutingModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
