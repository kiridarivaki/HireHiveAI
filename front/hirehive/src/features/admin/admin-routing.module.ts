import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from '@shared/layouts/main-layout.component.ts/main-layout.component';
import { CandidateMatchesComponent } from './candidate-matches/pages/candidate-matches.page';

const routes: Routes = [
    { path: 'results', component: CandidateMatchesComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}