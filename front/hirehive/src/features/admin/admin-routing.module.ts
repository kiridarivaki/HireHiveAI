import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateMatchesComponent } from './candidate-matches/pages/candidate-matches.page';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { JobProfileComponent } from './candidate-matches/pages/job-profile.page';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'results', component: CandidateMatchesComponent },
      { path: 'job-profile', component: JobProfileComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}