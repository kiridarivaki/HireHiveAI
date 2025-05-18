import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidatesMatchComponent } from './candidate-matches/pages/candidate-matches.page';

const routes: Routes = [
    { path: 'results', component: CandidatesMatchComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}