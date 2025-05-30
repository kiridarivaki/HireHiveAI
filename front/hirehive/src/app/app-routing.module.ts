import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '@shared/guards/auth.guard';
import { RoleGuard } from '@shared/guards/role.guard';
import { CoreRoutingModule } from 'src/features/core/core-routing.module';

const routes: Routes = [
  { 
    path: 'admin',
    canActivate: [AuthGuard, RoleGuard],
    loadChildren: () => import('../features/admin/admin.module').then(m => m.AdminModule)
  },
  { path: 'user', loadChildren: () => import('../features/user/user.module').then(m => m.UserModule) },
  { path: '', loadChildren: () => import('../features/core/core.module').then(m => m.CoreModule) },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes), CoreRoutingModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
