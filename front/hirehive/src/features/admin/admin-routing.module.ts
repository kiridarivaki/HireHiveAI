import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateMatchesComponent } from './candidate-matches/pages/candidate-matches.page';
import { JobProfileComponent } from './job-profile/pages/job-profile.page';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';

const routes: Routes = [{
  path: '',
  component: MainLayoutComponent,
  children: [
    { path: 'job-profile', component: JobProfileComponent },
    { path: 'results', component: CandidateMatchesComponent }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}