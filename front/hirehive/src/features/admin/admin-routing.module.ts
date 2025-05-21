import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateMatchesComponent } from './candidate-matches/pages/candidate-matches.page';
import { AuthGuard } from '@shared/guards/auth.guard';
import { RoleGuard } from '@shared/guards/role.guard';
import { JobProfileComponent } from './job-profile/pages/job-profile.page';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';

const routes: Routes = [{
  path: '',
  component: MainLayoutComponent,
  children: [
    { path: 'job-profile', component: JobProfileComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'results', component: CandidateMatchesComponent, canActivate: [AuthGuard, RoleGuard] }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}