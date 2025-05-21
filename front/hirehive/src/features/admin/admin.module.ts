import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { AdminRoutingModule } from "./admin-routing.module";
import { CandidateCardComponent } from "./candidate-matches/components/candidate-card/candidate-card.component";
import { MatCardModule } from "@angular/material/card";
import { CandidateMatchesComponent } from "./candidate-matches/pages/candidate-matches.page";

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    MatCardModule,
    CandidateMatchesComponent,
    CandidateCardComponent
]
})
export class AdminModule {}
