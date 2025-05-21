import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateMatchesComponent } from './candidate-matches/pages/candidate-matches.page';
import { AuthGuard } from '@shared/guards/auth.guard';
import { RoleGuard } from '@shared/guards/role.guard';

const routes: Routes = [
  { path: 'results', component: CandidateMatchesComponent, canActivate: [AuthGuard, RoleGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}