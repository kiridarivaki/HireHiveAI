import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { AdminRoutingModule } from "./admin-routing.module";
import { ResumePreviewComponent } from "./candidate-matches/components/resume-preview/resume-preview.component";
import { CandidatesMatchComponent } from "./candidate-matches/pages/candidate-matches.page";
import { CandidateCardComponent } from "./candidate-matches/components/candidate-card/candidate-card.component";

@NgModule({
  declarations: [CandidatesMatchComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ResumePreviewComponent,
    CandidateCardComponent
]
})
export class AdminModule {}
